using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WitchSpawn : MonoBehaviour {

    public GameObject witch;
    bool spawned = false;

    public void SpawnWitch()
    {
        Transform req = GameObject.Find("Witch_Requirements").transform;
        List<Transform> tel = new List<Transform>();
        List<Transform> sPos = new List<Transform>();
        tel.Add(req.FindChild("TeleportPositions").GetChild(0));
        tel.Add(req.FindChild("TeleportPositions").GetChild(1));
        sPos.Add(req.FindChild("EnemyPositions").GetChild(0));
        sPos.Add(req.FindChild("EnemyPositions").GetChild(1));
        GameObject g = Instantiate(witch, tel[0].position, Quaternion.identity) as GameObject;
        Witch w = g.GetComponent<Witch>();
        w.Init(sPos, tel);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && !spawned)
        {
            SpawnWitch();
            spawned = true;
        }
    }
}
