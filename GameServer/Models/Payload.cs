using System.Xml.Serialization;

public class Payload
{
    [XmlAttribute("code")]
    public short code;
    [XmlAttribute("value")]
    public string value;
    [XmlAttribute("clientID")]
    public string clientID;

    public override string ToString()
    {
        return "Payload: code: " + code + " value: " + value + " clientID: " + clientID;
    }
}
