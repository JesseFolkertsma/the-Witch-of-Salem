using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public Animator anim;
    public Rigidbody rb;

    public PlayerMovement pMove;
    public PlayerAttacks pa;
    public PlayerStats ps;

    public Transform playerMiddle;

    public LookAt la;
    public LookatPoint lp;

    public void Init (){
        pMove = GetComponent<PlayerMovement>();
        pa = GetComponent<PlayerAttacks>();
        ps = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        la = GetComponent<LookAt>();
        lp = GameObject.FindGameObjectWithTag("LookatObject").GetComponent<LookatPoint>();
        playerMiddle = GameObject.Find("Playermiddle").transform;
    }
}
