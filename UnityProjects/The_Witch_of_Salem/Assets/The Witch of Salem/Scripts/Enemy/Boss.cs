using UnityEngine;
using System.Collections;

public class Boss : Enemy {

    public Animator anim;
    public Rigidbody rb;
    
    public void BossStart()
    {
        player = FindObjectOfType<_PlayerBase>().transform;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }
}
