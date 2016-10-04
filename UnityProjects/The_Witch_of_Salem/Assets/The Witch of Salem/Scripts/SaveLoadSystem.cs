using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public class SaveLoadSystem {

	public void SaveGame()
    {
        SaveFile file = new SaveFile(GameObject.FindGameObjectWithTag("Player").transform.position);

        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));
        FileStream stream = new FileStream(Application.dataPath + "/The Witch of Salem/SaveFiles/SaveGame.xml", FileMode.Create);
        serializer.Serialize(stream, file);
        stream.Close();
    }

    public void LoadGame()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));
        FileStream stream = new FileStream(Application.dataPath + "/The Witch of Salem/SaveFiles/SaveGame.xml", FileMode.Open);
        SaveFile file = serializer.Deserialize(stream) as SaveFile;
        stream.Close();

        GameObject.FindGameObjectWithTag("Player").transform.position = file.playerPos;
    }
}
