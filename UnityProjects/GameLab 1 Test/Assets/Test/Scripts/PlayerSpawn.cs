using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawn : MonoBehaviour {
    public List<GameObject> playerRequirements;

    void Start()
    {
        List<GameObject> goList = new List<GameObject>();

        for(int i = 0; i < playerRequirements.Count; i++){
            goList.Add((GameObject)Instantiate(playerRequirements[i], transform.position, Quaternion.identity));
        }

        goList[0].GetComponent<LookatPoint>().Init();
        goList[1].GetComponent<PlayerManager>().Init();
    }

    public void SpawnPlayer()
    {
        List<GameObject> goList = new List<GameObject>();

        for (int i = 0; i < playerRequirements.Count; i++)
        {
            goList.Add((GameObject)Instantiate(playerRequirements[i], transform.position, Quaternion.identity));
        }

        goList[0].GetComponent<LookatPoint>().Init();
        goList[1].GetComponent<PlayerManager>().Init();
    }
}
