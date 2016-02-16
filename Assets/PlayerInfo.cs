using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerInfo
{
    int m_cash;

    public void Load()
    {
        BinarySerializer.ReadFromBinaryFile<PlayerInfo>(Application.dataPath + "SaveFile");
    }

    public void Save()
    {
        BinarySerializer.WriteToBinaryFile<PlayerInfo>(Application.dataPath + "SaveFile",this);
    }
}

public class BinarySerializer
{
    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }

    public static T ReadFromBinaryFile<T>(string filePath)
    {
        using (Stream stream = File.Open(filePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
    }
}