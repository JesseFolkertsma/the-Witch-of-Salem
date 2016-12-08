using UnityEngine;
using System.Collections;

public class Boss : Enemy {
    
    public void BossStart()
    {
        player = FindObjectOfType<_PlayerBase>().transform;
    }

}
