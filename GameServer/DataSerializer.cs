using System.IO;
using System.Xml.Serialization;

class DataSerializer
{
    public static byte[] SerializePayload(Payload p)
    {
        if (p == null)
        {
            return null;
        }

        MemoryStream ms = new MemoryStream();
        XmlSerializer s = new XmlSerializer(typeof(Payload));
        s.Serialize(ms, p);

        byte[] datagram = ms.GetBuffer();
        ms.Close();

        return datagram;
    }

    public static Payload DeserializePayload(byte[] datagram)
    {
        if (datagram == null)
        {
            return null;
        }

        XmlSerializer s = new XmlSerializer(typeof(Payload));
        MemoryStream ms = new MemoryStream(datagram);
        Payload p = (Payload)s.Deserialize(ms);
        ms.Close();

        return p;
    }
}
