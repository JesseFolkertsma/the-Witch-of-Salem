using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
public class SaveFile  {

    public Vector3 playerPos;
    public int lives;
    public int apples;
    public int arrows;

    public string levelName;
    public int levelID;

    public List<bool> crates;
    public List<bool> spawns;
    public List<bool> messages;


    public SaveFile(Vector3 pos, int _lives, int _apples, int _arrows, List<BreakableLootObject> _crates, List<EnemySpawnEvent> _spawns, List<Message> _messages, string _levelName, int _levelID)
    {
        playerPos = pos;
        lives = _lives;
        apples = _apples;
        arrows = _arrows;

        levelName = _levelName;
        levelID = _levelID;

        crates = new List<bool>();
        for (int i = 0; i < _crates.Count; i++)
        {
            if (_crates[i] != null)
            {
                crates.Add(false);
            }
            else
            {
                crates.Add(true);
            }
        }

        spawns = new List<bool>();
        for (int i = 0; i < _spawns.Count; i++)
        {
            spawns.Add(_spawns[i].isDone);
        }

        messages = new List<bool>();
        for (int i = 0; i < _messages.Count; i++)
        {
            if(_messages[i] != null)
                messages.Add(_messages[i].isDone);
        }
    }

    public SaveFile() { }

}
