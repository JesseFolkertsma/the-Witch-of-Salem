using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public class SaveLoadSystem {

    string savePath = "/The Witch of Salem/SaveFiles/";

    public GameManager gm;

    public SaveFile sFile;

    SaveFile SetupSaveFile()
    {
        SaveFile file = new SaveFile(GameObject.FindGameObjectWithTag("Player").transform.position, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>(), gm.currentCheckpoint);
        return file;
    }

    void SetupLoad(SaveFile file)
    {
        gm.loadPos = file.playerPos;
        gm.playerS.lives = file.lives;
        gm.playerS.apples = file.apples;
        gm.playerS.arrows = file.arrows;
        gm.currentLevel = file.currentlevel;
        gm.currentCheckpoint = file.currentCheckpoint;
        //SetupLevel(file.currentlevel, file.currentCheckpoint);
    }

    public void NewGame()
    {
        LoadGame("NewGame");
        SceneManager.LoadScene(1);
    }

    public void LoadScene(string pn)
    {
        LoadGame(pn);
        SceneManager.LoadScene(1);
    }

	public void SaveGame(string pn)
    {
        SaveFile file = SetupSaveFile();

        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));
        //FileStream stream = new FileStream(Application.dataPath + savePath + pn + "_SaveGame.xml", FileMode.Create);
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + pn + "_SaveGame.xml", FileMode.Create);
        serializer.Serialize(stream, file);
        stream.Close();
    }

    public void LoadGame(string pn)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));
        //FileStream stream = new FileStream(Application.dataPath + savePath + pn + "_SaveGame.xml", FileMode.Open);
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + pn + "_SaveGame.xml", FileMode.Open);
        SaveFile file = serializer.Deserialize(stream) as SaveFile;
        stream.Close();

        SetupLoad(file);
        sFile = file;
    }

    public void SetupLevel(int level, int checkpoint)
    {
        switch (level)
        {
            case 0:
                SetupTutorial(checkpoint);
                break;
            case 1:
                SetupFarmlands(checkpoint);
                break;
            case 2:
                SetupForest(checkpoint);
                break;
            case 3:
                SetupCaves(checkpoint);
                break;
            case 4:
                SetupWitchTower(checkpoint);
                break;
            case 5:
                SetupTest(checkpoint);
                break;
        }
    }

    void SetupTutorial(int checkpoint)
    {

    }

    void SetupFarmlands(int checkpoint)
    {

    }

    void SetupForest(int checkpoint)
    {

    }
    void SetupCaves(int checkpoint)
    {

    }
    void SetupWitchTower(int checkpoint)
    {

    }
    void SetupTest(int checkpoint)
    {
        if(checkpoint >= 1)
        {
            GameObject.Find("M_Bridge").GetComponent<BridgeRope>().BreakRope();
            if(checkpoint >= 2)
            {

            }
        }
    }
}
