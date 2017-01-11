using UnityEngine;
using System.Collections;

public class _GameManager : MonoBehaviour {

    public static _GameManager instance;
    public SaveLoadSystem saveSystem;
    public LevelData levelData;

    public string playerName;

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

    }

    public void LoadLevelData(SaveFile file)
    {
        _Player p = FindObjectOfType<_Player>();
        p.transform.position = file.playerPos;
        p.lives = file.lives;
        p.apples = file.apples;
        p.arrows = file.arrows;
        _UIManager.instance.UpdateUI();

        for (int i = 0; i < levelData.crates.Count; i++)
        {
            levelData.crates[i].broken = file.crates[i];
        }
        for (int i = 0; i < levelData.spawners.Count; i++)
        {
            levelData.spawners[i].isDone = file.spawns[i];
        }
    }
}
