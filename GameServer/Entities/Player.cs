using System.Net;

public class Player
{
    public string address;
    public int port;
    public string clientID;
    public IPEndPoint endPoint;

    public Player(IPEndPoint endPoint)
    {
        this.endPoint = endPoint;
        this.address = endPoint.Address.ToString();
        this.port = endPoint.Port;
        this.clientID = System.Guid.NewGuid().ToString();
    }
}