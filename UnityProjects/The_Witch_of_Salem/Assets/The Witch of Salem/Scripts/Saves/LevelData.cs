using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LevelData : MonoBehaviour {

    public bool gatherLevelData = false;

    public string levelName;

    public List<BreakableLootObject> crates;
    public List<EnemySpawnEvent> spawners;

    void Update()
    {
        if (gatherLevelData)
        {
            GatherLevelData();
        }
        if (Application.isPlaying)
        {
            if(_GameManager.instance.levelData != this)
            {
                _GameManager.instance.levelData = this;
            }

            if (Input.GetButtonDown("Save"))
            {
                _GameManager.instance.saveSystem.SaveGame(_GameManager.instance.playerName, levelName, SceneManager.GetActiveScene().buildIndex, FindObjectOfType<_Player>(), this);
            }
            if (Input.GetButtonDown("Load"))
            {
                _GameManager.instance.LoadLevelWithSave();
            }
        }
    }

    void GatherLevelData()
    {
        gatherLevelData = false;
        crates.Clear();
        spawners.Clear();

        foreach (BreakableLootObject c in FindObjectsOfType<BreakableLootObject>())
        {
            crates.Add(c);
        }

        foreach (EnemySpawnEvent e in FindObjectsOfType<EnemySpawnEvent>())
        {
            spawners.Add(e);
        }
    }
}
