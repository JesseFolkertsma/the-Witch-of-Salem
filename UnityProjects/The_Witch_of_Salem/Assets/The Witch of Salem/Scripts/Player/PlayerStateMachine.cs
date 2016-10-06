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

    public enum CombatState
    {
        Melee,
        Ranged
    };

    public State state = State.Walking;
    public CombatState combatState = CombatState.Melee;

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
    public bool jumpAttack;

    public Transform backBone;
    public GameObject shield;
    public GameObject bow;
    public GameObject arrow;
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
            case State.Aiming:
                pc.DrawArrow();
                break;
        }

        if(jumpAttack == true)
        {
            pc.JumpAttack();
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
                pm.Climb();
            }

            if(!Physics.Raycast(transform.position + new Vector3(0,1.5f,0), dir, .5f) && ray.transform.tag == "Ledge")
            {
                state = State.Climbing;
                pm.Climb();
            }
            else if (ray.transform.tag == "Ledge")
            {
                pm.DropClimb();
            }
        }
        else if(isClimbing == true)
        {
            pm.ClimbUp();
        }

        //Stuff for weaponswapping
        if (Input.GetButtonDown("Fire3"))
        {
            SwapWeapons();
        }
    }

    public void ToAttack()
    {
        if (combatState == CombatState.Melee)
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
                    pc.JumpAttackInit();
                    break;
                case State.Blocking:
                    pc.BasicAttack();
                    break;
            }
        }

        if(combatState == CombatState.Ranged)
        {
            switch (state)
            {
                case State.Aiming:
                    pc.ShootArrow();
                    break;
            }
        }
    }

    public void SwapWeapons()
    {
        if(combatState == CombatState.Melee)
        {
            combatState = CombatState.Ranged;
            bow.SetActive(true);
        }

        else if (combatState == CombatState.Ranged)
        {
            combatState = CombatState.Melee;
            bow.SetActive(false);
        }
    }
}
