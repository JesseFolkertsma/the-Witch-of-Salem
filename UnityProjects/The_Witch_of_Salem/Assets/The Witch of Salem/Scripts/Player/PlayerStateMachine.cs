using UnityEngine;
using System.Collections;

public class PlayerStateMachine : MonoBehaviour {

    public enum State
    {
        CantMove,
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
        Ranged,
        Unarmed
    };

    public State state = State.Walking;
    public CombatState combatState = CombatState.Melee;

    public PlayerStats ps;
    public PlayerMovements pm;
    public PlayerCombat pc;
    public PlayerIK pIK;
    
    [HideInInspector]
    public Vector3 dir;

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rb;
    public RaycastHit ray;

    public float movementSpeed;
    public float checkForFloorRange = 1.1f;
    public float climbSpeed;

    public bool isFalling;
    public bool isClimbing;
    public bool jumpAttack;
    public bool isDead;

    public Transform model;
    public JonasWeapons weapons;
    public GameObject arrow;
    public Transform mouse;

    public GameObject walkingPar;
    public ParticleSystem wParSystem;
    public GameObject smashParticles;

    public GameObject ragdoll;

    Transform playerM;

    void Start()
    {
        GameManager.instance.InitPlayer();
        pm = new PlayerMovements(this);
        pc = new PlayerCombat(this);
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<PlayerStats>();
        pIK = GetComponentInChildren<PlayerIK>();
        walkingPar = Instantiate(walkingPar, transform.position , Quaternion.identity) as GameObject;
        walkingPar.transform.parent = transform;
        wParSystem = walkingPar.GetComponent<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();
        playerM = transform.FindChild("PlayerMiddle");
        pc.bowAnim = weapons.bow.GetComponent<Animator>();
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
            case State.Attacking:
                pc.WhileAttacking();
                break;
        }

        GameManager.instance.pUI.lives = ps.lives;
        GameManager.instance.pUI.arrows = ps.arrows;
        GameManager.instance.pUI.apples = ps.apples;

        if (ps.lives <= 0)
        {
            Die();
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
        anim.SetFloat("Movement", pm.movement.x);
        anim.SetBool("IsFalling", isFalling);
        anim.SetInteger("Combo", pc.currentCombo);

        //Check for falling
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, checkForFloorRange))
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

        if (Input.GetButtonDown("Q"))
        {
            if (ps.apples > 0 && ps.lives < 5)
            {
                ps.lives++;
                ps.apples--;
            }
        }
    }

    public void ToAttack()
    {
        if (combatState == CombatState.Melee)
        {
            switch (state)
            {
                case State.Walking:
                    pc.ComboAttack(Random.Range(1, 4));
                    state = State.Attacking;
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
                    pc.ComboAttack(Random.Range(1, 4));
                    break;
            }
        }

        if(combatState == CombatState.Ranged)
        {
            switch (state)
            {
                case State.Aiming:
                    break;
            }
        }
    }

    public void SwapWeapons()
    {
        if(combatState == CombatState.Melee && ps.hasBow == true)
        {
            combatState = CombatState.Ranged;
            anim.SetLayerWeight(2, 1);
            weapons.bow.SetActive(true);
            weapons.endSword.SetActive(false);
            weapons.endShield.SetActive(false);
        }

        else if (combatState == CombatState.Ranged)
        {
            combatState = CombatState.Melee;
            anim.SetLayerWeight(2, 0);
            weapons.bow.SetActive(false);
            weapons.endSword.SetActive(true);
            weapons.endShield.SetActive(true);
        }
    }

    public void Die()
    {
        if (isDead == false)
        {
            Instantiate(ragdoll, transform.position, transform.rotation);
            isDead = true;
        }
    }
}
