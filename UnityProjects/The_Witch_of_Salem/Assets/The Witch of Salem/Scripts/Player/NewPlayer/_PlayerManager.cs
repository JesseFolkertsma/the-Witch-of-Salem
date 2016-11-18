using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _PlayerManager : MonoBehaviour {

    public static _PlayerManager instance;
    public List<_PlayerBase> players;
    public bool singlePlayer;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
