using System;
using System.Net;
using System.Net.Sockets;

public class MultiplayerGame
{
    public int index;
    public bool isReady = false;
    public Player p1 = null;
    public Player p2 = null;

    public MultiplayerGame(int index)
    {
        this.index = index;
        UIConsoleManager.WriteLineInCyan ("\n>>Game {0}: created", index);
    }

    public void DoLogics(UdpClient udpClient, string clientID, byte[] datagram)
    {
        if (isReady)
        {
            if (clientID == p1.clientID)
            {
                udpClient.Send(datagram, datagram.Length, p2.endPoint);
            }

            if (clientID == p2.clientID)
            {
                udpClient.Send(datagram, datagram.Length, p1.endPoint);
            }
        }
    }

    public void AddPlayer(Player p)
    {
        if (!isReady)
        {
            if (p1 == null)
            {
                UIConsoleManager.WriteLineInGreen("Player 1 -> {0} has joined Game {1}", p.endPoint.ToString(), index);
                p1 = p;
            }
            else if (p2 == null && p.clientID != p1.clientID)
            {
                UIConsoleManager.WriteLineInGreen("Player 2 -> {0} has joined Game {1}", p.endPoint.ToString(), index);
                p2 = p;
            }

            if (p1 != null && p2 != null)
            {
                UIConsoleManager.WriteLineInCyan("\n>>Game {0}: started running", index);
                isReady = true;
            }
        }
    }
}
