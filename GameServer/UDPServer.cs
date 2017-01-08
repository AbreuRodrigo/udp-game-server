using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace GameServer
{
    public class UDPServer
    {
        private Dictionary<IPEndPoint, int> playerList = new Dictionary<IPEndPoint, int>();
        private List<MultiplayerGame> games = new List<MultiplayerGame>();
        private int port = 11000;
        private bool running = false;

        private Thread mainThread;
        private UdpClient listener;
        private MultiplayerGame currentGame;

        public delegate void ServerAction(ServerData data);
        private Dictionary<short, ServerAction> actionToTake = null;

        private int currentGameIndex = 1;

        //Constructor
        public UDPServer(int port)
        {
            this.port = port;

            Initialize();
        }

        //Initialize action dictionary
        private void Initialize()
        {
            listener = new UdpClient(port);
            mainThread = new Thread(MainThread);

            actionToTake = new Dictionary<short, ServerAction>(2)
            {
                {1, CreateNewPlayer}, {2, DoGameLogics}
            };
        }

        //Start Server
        public void Start()
        {
            UIConsoleManager.WriteLineInYellow("Starting server...");

            running = true;

            mainThread.Start();
        }

        //On finish application
        private void OnProcessExit(object sender, EventArgs e)
        {
            running = false;

            if (mainThread != null)
            {
                mainThread.Abort();
            }

            UIConsoleManager.WriteLineInRed("Server stopped");
        }

        //Main thread that keeps waiting/handling players data
        private void MainThread()
        {
            try
            {
                UIConsoleManager.WriteLineInGreen("Server has started...");

                ServerData data = null;

                while (running)
                {
                    data = new ServerData();
                    data.endPoint = new IPEndPoint(IPAddress.Any, 0);
                    data.datagram = listener.Receive(ref data.endPoint);

                    HandleDatagram(data);
                }
            }
            catch (System.Exception e)
            {
                UIConsoleManager.WriteLineInRed(e.Message);
            }
        }

        //Handle the datagram from the players
        private void HandleDatagram(ServerData data)
        {
            Payload payload = DataSerializer.DeserializePayload(data.datagram);
            short code = payload.code;

            try
            {
                if (payload != null && actionToTake != null)
                {
                    actionToTake[code](data);
                }
            }
            catch (Exception e)
            {
                UIConsoleManager.WriteLineInRed("Error: Invalid code sent by player - Code: " + code);
                UIConsoleManager.WriteLineInDarkRed("Original Message: " + e.Message);
            }
        }

        private MultiplayerGame GetAvailableGame()
        {
            if (currentGame == null)
            {
                currentGame = new MultiplayerGame(currentGameIndex);
            }

            //Not available situation
            if (currentGame.isReady)
            {
                RecycleCurrentGame();
            }

            return currentGame;
        }

        private void RecycleCurrentGame()
        {
            games.Add(currentGame);

            currentGameIndex++;

            currentGame = new MultiplayerGame(currentGameIndex);
        }

        private Payload DatagramToPayload(byte[] datagram)
        {
            if (datagram == null)
            {
                return null;
            }

            return DataSerializer.DeserializePayload(datagram);
        }

        //SERVER ACTIONS

        private void CreateNewPlayer(ServerData data)
        {
            Payload payload = DatagramToPayload(data.datagram);
            currentGame = GetAvailableGame();

            if (payload != null && string.IsNullOrEmpty(payload.clientID))
            {
                Player player = new Player(data.endPoint);
                currentGame.AddPlayer(player);
                playerList.Add(player.endPoint, currentGame.index - 1);

                payload.clientID = player.clientID;
                data.datagram = DataSerializer.SerializePayload(payload);

                listener.Send(data.datagram, data.datagram.Length, data.endPoint.Address.ToString(), data.endPoint.Port);
            }

            if (currentGame.isReady)
            {
                RecycleCurrentGame();
            }
        }

        private void DoGameLogics(ServerData data)
        {
            Payload payload = DatagramToPayload(data.datagram);

            if (payload != null && !string.IsNullOrEmpty(payload.clientID))
            {
                int gamesRunning = games.Count;

                if (gamesRunning > 0)
                {
                    int gameIndex = playerList[data.endPoint];

                    if (gameIndex <= gamesRunning-1) {
                        MultiplayerGame game = games[gameIndex];

                        if (game != null && game.isReady)
                        {
                            game.DoLogics(listener, payload.clientID, data.datagram);
                        }
                    }
                }
            }
        }
    }
}