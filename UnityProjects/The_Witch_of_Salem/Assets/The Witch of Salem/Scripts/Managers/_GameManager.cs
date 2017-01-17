using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class _GameManager : MonoBehaviour {

    public static _GameManager instance;
    public SaveLoadSystem saveSystem;
    public LevelData levelData;

    public string playerName;

    public SaveFile file;
    public bool willLoadData;
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

    public void LoadLevelWithSave()
    {
        file = saveSystem.LoadGame(playerName, false);
        willLoadData = true;
        SceneManager.LoadScene(file.levelID);
    }

    public void LoadOtherLevelWithSave(int buildIndex)
    {
        file = saveSystem.LoadGame(playerName, false);
        willLoadData = true;
        SceneManager.LoadScene(buildIndex);
    }

    void OnLevelWasLoaded()
    {
        if (willLoadData)
        {
            LoadLevelData(file, false);
            willLoadData = false;
        }
    }

    public void LoadLevelData(SaveFile _file, bool playerDataOnly)
    {
        levelData = FindObjectOfType<LevelData>();

        _Player p = FindObjectOfType<_Player>();
        p.transform.position = file.playerPos;
        p.lives = file.lives;
        p.apples = file.apples;
        p.arrows = file.arrows;
        print("Loaded player data");

        if (!playerDataOnly)
        {
            for (int i = 0; i < levelData.crates.Count; i++)
            {
                levelData.crates[i].broken = file.crates[i];
            }
            for (int i = 0; i < levelData.spawners.Count; i++)
            {
                levelData.spawners[i].isDone = file.spawns[i];
            }
            print("LoadedLevel data");
        }

        _UIManager.instance.UpdateUI();
    }
}
