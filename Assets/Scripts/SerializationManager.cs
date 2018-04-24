using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using System.Collections.Generic;

public static class SerializationManager
{

    public static List<ISerializationSurrogate> Surrogates = new List<ISerializationSurrogate>();
    public static SurrogateSelector Selector = new SurrogateSelector();
    static SerializationManager()
    {
        Selector.AddSurrogate(typeof(Color32), new StreamingContext(StreamingContextStates.All), new Color32SerializationSurrogate());
    }

    public static string CreatePath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, "SaveData/"+filename);
    }

    public static byte[] SerializeObject(object graph)
    {
        MemoryStream ms = new MemoryStream();
        SaveObject(ms, graph);
        ms.Flush();
        byte[] array = ms.ToArray();
        ms.Dispose();
        return array;
    }

    public static bool SaveObject(Stream stream, object graph)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.SurrogateSelector = Selector;

        try
        {
            formatter.Serialize(stream, graph);
        }
        catch (IOException)
        {
            return false;
        }
        return true;

    }
    public static bool SaveObject(string path, object graph)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string folderPath = Path.GetDirectoryName(path);
        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

        }
        catch (IOException)
        {
            return false;
        }
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.SurrogateSelector = Selector;

            try
            {
                formatter.Serialize(stream, graph);
            }
            catch (IOException)
            {
                return false;
            }
        }
        Debug.LogFormat("Saved at: {0}", path);
        return true;
    }

    public static object LoadObject(byte[] array)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (MemoryStream stream = new MemoryStream(array))
        {

            formatter.SurrogateSelector = Selector;
            try
            {
                return formatter.Deserialize(stream);
            }
            catch (IOException)
            {
                return null;
            }
        }
    }


    public static object LoadObject(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogFormat("Could not load file: {0}\nFile does not exist!", path);
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            formatter.SurrogateSelector = Selector;
            try
            {
                Debug.LogFormat("Loading file: {0}", path);
                return formatter.Deserialize(stream);
            }
            catch (IOException)
            {
                return null;
            }
        }
    }
    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            Debug.LogFormat("Deleting file: {0}", path);
            File.Delete(path);
        }
        else
        {
            Debug.LogFormat("Could not delete file: {0}\nFile does not exist!", path);
        }
        
    }
}
