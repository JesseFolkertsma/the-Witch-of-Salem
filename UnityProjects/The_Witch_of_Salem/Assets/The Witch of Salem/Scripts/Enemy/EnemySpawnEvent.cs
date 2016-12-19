using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnEvent : MonoBehaviour {

    public List<EnemySpawn> spawns;
    public GameObject col;

    public bool isDone;
    bool active;

    void Start()
    {
        col.SetActive(false);
    }

    public void Activate()
    {
        active = true;
        col.SetActive(true);
        for(int i = 0; i < spawns.Count; i++)
        {
            spawns[i].Spawn(this);
        }
    }

    void End()
    {
        isDone = true;
        active = false;
        isDone = true;
        col.SetActive(false);
    }

    public void CheckForEnd()
    {
        bool allDead = true;
        for(int i = 0; i < spawns.Count; i++)
        {
            if (!spawns[i].isDead)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            End();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        print("SPAWN");
        if(!isDone && !active)
        {
            if (col.attachedRigidbody)
            {
                if (col.attachedRigidbody.GetComponent<_PlayerBase>())
                {
                    Activate();
                }
            }
        }
    }
}
