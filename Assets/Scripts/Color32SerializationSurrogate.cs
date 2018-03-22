using UnityEngine;
using System.Runtime.Serialization;

public class Color32SerializationSurrogate : ISerializationSurrogate
{
    #region ISerializationSurrogate implementation

    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Color32 c = (Color32)obj;
        /*
        info.AddValue("r",c.r);
        info.AddValue("g",c.g);
        info.AddValue("b",c.b);
        info.AddValue("a",c.a);
        */
        info.AddValue("v", new byte[] { c.r, c.g, c.b, c.a }, typeof(byte[]));
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Color32 c = (Color32)obj;
        byte[] values = (byte[])info.GetValue("v", typeof(byte[]));
        c.r = values[0]; c.g = values[1]; c.b = values[2]; c.a = values[3];
        /*
        c.r = info.GetByte("r");
        c.g = info.GetByte("g");
        c.b = info.GetByte("b");
        c.a = info.GetByte("a");
        */
        return (c);
    }

    #endregion
}
