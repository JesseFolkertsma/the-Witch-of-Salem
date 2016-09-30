using UnityEngine;
using System.Collections;

public class PlayerStateMachine : MonoBehaviour {

    public enum State
    {
        Walking,
        Running,
        Crouching,
        Climbing,
        Falling,
        Attacking,
        Blocking,
        Aiming
    };

    public State state = State.Walking;

    public PlayerMovements pm;
    public PlayerCombat pc;
    
    public Vector3 dir;

    public Animator anim;
    public Rigidbody rb;
    public RaycastHit ray;

    public float movementSpeed;
    public float climbSpeed;

    public bool isFalling;
    public bool isClimbing;

    public Transform backBone;
    public GameObject shield;
    public Transform mouse;

    void Awake()
    {
        pm = new PlayerMovements(this);
        pc = new PlayerCombat(this);
        rb = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        PlayerInput();

        switch (state)
        {
            case State.Walking:
                pm.Walk();
                break;
            case State.Running:
                pm.Run();
                break;
            case State.Crouching:
                pm.Crouch();
                break;
            case State.Falling:
                pm.Falling();
                break;
            case State.Blocking:
                pc.Block();
                break;
        }
    }

    void PlayerInput()
    {
        //Setup movement and check witch direction player is facing
        pm.movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        //Check for falling
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            isFalling = false;
            if(state == State.Falling)
            {
                state = State.Walking;
            }
        }
        else
        {
            isFalling = true;
            if(isClimbing == false)
            {
                state = State.Falling;
            }
        }

        //Check for climbable object
        if (Physics.Raycast(transform.position, dir, out ray, .5f))
        {
            if(ray.transform.tag == "Ladder")
            {
                state = State.Climbing;
                pm.ClimbLadder();
            }
        }
        else if(isClimbing == true)
        {
            pm.ClimbUp();
        }
        Debug.DrawRay(transform.position, dir, Color.red);
    }

    public void ToAttack()
    {
        switch (state)
        {
            case State.Walking:
                pc.BasicAttack();
                break;
            case State.Running:
                pc.SprintAttack();
                break;
            case State.Crouching:
                print("Cant attack while crouching!");
                break;
            case State.Falling:
                pc.JumpAttack();
                break;
            case State.Blocking:
                pc.BasicAttack();
                break;
        }
    }
}
