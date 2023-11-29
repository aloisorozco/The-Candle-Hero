using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Data data, string dataFile)
    {
        BinaryFormatter formater = new BinaryFormatter();
        string path = Application.persistentDataPath + "/"+dataFile;
        FileStream stream = new FileStream(path, FileMode.Create);

        formater.Serialize(stream, data);
        stream.Close();

    }

    public static Data LoadPlayer(string dataFile)
    {
        string path = Application.persistentDataPath + "/" + dataFile;
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
            return null;
        }
        
    }

    public static Data NewGame()
    {
        return new Data();
    }

    public static void DeleteSaveFiles()
    {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        foreach (string filePath in filePaths)
        {
            File.Delete(filePath);
        }
            
    }

    public static bool FindSaveFile()
    {
        if (Directory.GetFiles(Application.persistentDataPath).Length > 0){
            return true;
        }
        else { return false; }
    }

    public static int GetFileNum()
    {
        return Directory.GetFiles(Application.persistentDataPath).Length + 1;
    }
}
