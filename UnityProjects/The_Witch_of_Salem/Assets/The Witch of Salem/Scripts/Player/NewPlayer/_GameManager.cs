using UnityEngine;
using System.Collections;

public class _GameManager : MonoBehaviour {

    public _PlayerManager pMangager;

    void Awake()
    {
        pMangager = new _PlayerManager();
    }
}
