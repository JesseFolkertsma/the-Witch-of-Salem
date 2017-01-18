using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class _GameManager : MonoBehaviour {

    public static _GameManager instance;
    public SaveLoadSystem saveSystem;
    public LevelData levelData;

    public string playerName;

    public SaveFile file;
    public bool willLoadData;
    public bool playerDataOnly = false;
    bool playerIsDead = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
        saveSystem = new SaveLoadSystem();
        saveSystem.gm = this;
    }

    void Update()
    {
        if (playerIsDead)
        {
            if (Input.GetButtonDown("Jump"))
            {
                LoadLevelWithSave();
            }
        }
    }

    public void PlayerDeath(bool isdead)
    {
        if (isdead)
        {
            playerIsDead = true;
        }
        else
        {
            playerIsDead = false;
        }
    }

    public void SaveGame()
    {
        levelData = FindObjectOfType<LevelData>();
        _Player player = FindObjectOfType<_Player>();
        saveSystem.SaveGame(playerName, levelData.levelName, SceneManager.GetActiveScene().buildIndex, player, levelData);
    }

    public void LoadLevelWithSave()
    {
        file = saveSystem.LoadGame(playerName);
        willLoadData = true;
        playerDataOnly = false;
        SceneManager.LoadScene(file.levelID);
    }

    public void LoadOtherLevelWithSave(int buildIndex)
    {
        file = saveSystem.LoadGame(playerName);
        willLoadData = true;
        playerDataOnly = true;
        SceneManager.LoadScene(buildIndex);
    }

    void OnLevelWasLoaded()
    {
        print("Will load level data = " + willLoadData);
        if (willLoadData)
        {
            LoadLevelData(file, playerDataOnly);
            willLoadData = false;
        }
    }

    public void LoadLevelData(SaveFile _file, bool playerDataOnly)
    {
        levelData = FindObjectOfType<LevelData>();
        levelData.messages.Clear();
        levelData.messages = new List<Message>(FindObjectsOfType<Message>());

        _Player p = FindObjectOfType<_Player>();
        p.lives = file.lives;
        p.apples = file.apples;
        p.arrows = file.arrows;
        print("Loaded player data");

        if (!playerDataOnly)
        {
            p.transform.position = file.playerPos;
            for (int i = 0; i < levelData.crates.Count; i++)
            {
                levelData.crates[i].broken = file.crates[i];
            }
            for (int i = 0; i < levelData.spawners.Count; i++)
            {
                levelData.spawners[i].isDone = file.spawns[i];
            }
            for (int i = 0; i < levelData.messages.Count; i++)
            {
                if (levelData.messages[i] != null)
                {
                    levelData.messages[i].SetIsDone(file.messages[i]);
                }
            }
            print("LoadedLevel data");
        }

        _UIManager.instance.UpdateUI();
    }
}
