using UnityEngine;
using System.Collections;

public class PlayerStateMachine : MonoBehaviour {

    public enum State
    {
        Idle,
        Walking,
        Running,
        Crouching,
        Climbing,
        Falling,
        Attacking
    };

    State state = State.Walking;

    PlayerMovements pm;
    PlayerCombat pc;

    Vector3 movement;

    Animator anim;
    Rigidbody rb;

    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float climbSpeed;

    void Awake()
    {
        pm = new PlayerMovements(this);
        pc = new PlayerCombat(this);
        rb = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        pm.movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    }
}
