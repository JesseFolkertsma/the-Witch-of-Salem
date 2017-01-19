using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public class SaveLoadSystem {

    string savePath = "/Recources";

    public _GameManager gm;

    //public SaveFile sFile;

    SaveFile SetupSaveFile(string pn, string levelName, int levelID, _Player player, LevelData lData)
    {
        SaveFile file = new SaveFile(player.transform.position, player.lives, player.apples, player.arrows, lData.crates, lData.spawners, lData.messages, levelName, levelID); 
        return file;
    }

	public void SaveGame(string pn, string levelName, int levelID, _Player player, LevelData lData)
    {
        SaveFile file = SetupSaveFile(pn, levelName, levelID, player, lData);
        //_GameManager.instance.file = file;

        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));
        Debug.Log("Saving to: " + Application.persistentDataPath + "/_SaveGame.xml");
        FileStream stream = new FileStream(Application.persistentDataPath + "/_SaveGame.xml", FileMode.Create);
        //FileStream stream = new FileStream(Application.persistentDataPath + "/" + pn + "_SaveGame.xml", FileMode.Create);
        serializer.Serialize(stream, file);
        stream.Close();
        Debug.Log("Saved Game!");
    }

    public SaveFile LoadGame(string pn)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));
        FileStream stream = new FileStream(Application.persistentDataPath + "/_SaveGame.xml", FileMode.Open);
        //FileStream stream = new FileStream(Application.persistentDataPath + "/" + pn + "_SaveGame.xml", FileMode.Open);
        SaveFile file = serializer.Deserialize(stream) as SaveFile;
        stream.Close();

        return file;
    }
}
