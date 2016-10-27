using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public class SaveLoadSystem {

    string savePath = "/The Witch of Salem/SaveFiles/";

    SaveFile SetupSaveFile()
    {
        SaveFile file = new SaveFile(GameObject.FindGameObjectWithTag("Player").transform.position, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>(), GameManager.instance.currentCheckpoint);
        return file;
    }

    void SetupLoad(SaveFile file)
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = file.playerPos;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().lives = file.lives;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().apples = file.apples;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().arrows = file.arrows;
        SetupLevel(file.currentlevel, file.currentCheckpoint);
    }

	public void SaveGame()
    {
        SaveFile file = SetupSaveFile();

        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));
        FileStream stream = new FileStream(Application.dataPath + savePath + "SaveGame.xml", FileMode.Create);
        serializer.Serialize(stream, file);
        stream.Close();
    }

    public void LoadGame()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));
        FileStream stream = new FileStream(Application.dataPath + savePath + "SaveGame.xml", FileMode.Open);
        SaveFile file = serializer.Deserialize(stream) as SaveFile;
        stream.Close();

        SetupLoad(file);
    }

    public void SetupLevel(int level, int checkpoint)
    {

    }
}
