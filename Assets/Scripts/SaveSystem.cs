using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string dataFile = "/player.lol";
    public static void SavePlayer(Data data)
    {
        BinaryFormatter formater = new BinaryFormatter();
        string path = Application.persistentDataPath + dataFile;
        FileStream stream = new FileStream(path, FileMode.Create);

        formater.Serialize(stream, data);
        stream.Close();

    }

    public static Data LoadPlayer()
    {
        Debug.Log("In");
        string path = Application.persistentDataPath + dataFile;
        if(File.Exists(path)) 
        {
            BinaryFormatter formater = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Data data = formater.Deserialize(stream) as Data;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found");
            return new Data();
        }
        
    }
}
