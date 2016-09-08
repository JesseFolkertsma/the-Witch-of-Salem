using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public Animator anim;
    public Rigidbody rb;

    public PlayerMovement pm;
    public PlayerAttacks pa;
    public PlayerStats ps;

    public LookatPoint lp;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        pa = GetComponent<PlayerAttacks>();
        ps = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        lp = GameObject.Find("LookatObject").GetComponent<LookatPoint>();
    }
}
