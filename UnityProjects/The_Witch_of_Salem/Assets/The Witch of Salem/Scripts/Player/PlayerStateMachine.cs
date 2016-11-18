using UnityEngine;
using System.Collections;

public class PlayerStateMachine : MonoBehaviour {

    public enum BaseState
    {
        CantMove,
        Running,
        Falling,
        Climbing,
    };

    public enum State
    {
        Idle,
        Blocking,
        Attacking,
        Aiming,
        JumpAttacking,
        Rolling
    }

    public enum CombatState
    {
        Melee,
        Ranged,
        Unarmed
    };

    public BaseState baseState = BaseState.Running;
    public State state = State.Idle;
    public CombatState combatState = CombatState.Melee;

    public PlayerStats ps;
    public PlayerMovements pm;
    public PlayerCombat pc;
    public PlayerIK pIK;

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
    public bool dirMouseBased;
    public Transform model;
    public JonasWeapons weapons;
    public GameObject arrow;
    public Transform mouse;

    public GameObject walkingPar;
    public ParticleSystem wParSystem;
    public GameObject smashParticles;
    public GameObject blood;

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
        weapons.bow.SetActive(false);
    }

    void Update ()
    {
        if (baseState != BaseState.CantMove)
        {
            PlayerInput();
            pc.CombatUpdate();

            switch (baseState)
            {
                case BaseState.Running:
                    pm.Run();
                    break;
                case BaseState.Falling:
                    pm.Falling();
                    break;
            }
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

    public int Direction ()
    {
        int dir = 0;
        if (dirMouseBased)
        {
            if (mouse.position.x > transform.position.x)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
        }
        else if(!dirMouseBased)
        {
            if (pm.movement.x < 0)
            {
                dir = -1;
            }
            else if (pm.movement.x > 0)
            {
                dir = 1;
            }
        }
        return dir;
    }

    public void ResetPlayer()
    {
        state = State.Idle;
        baseState = BaseState.Running;
    }

    void PlayerInput()
    {
        //Setup movement and check witch direction player is facing
        pm.movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        anim.SetFloat("Movement", pm.movement.x);
        anim.SetBool("IsFalling", isFalling);
        anim.SetBool("Attacking", pc.attacking);
        anim.SetInteger("Combo", pc.currentCombo);

        //Check for falling
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, checkForFloorRange))
        {
            isFalling = false;
            baseState = BaseState.Running;
        }
        else
        {
            isFalling = true;
            baseState = BaseState.Falling;
        }

        //Check for climbable object
        if (Physics.Raycast(transform.position, new Vector3(Direction(),0,0), out ray, .5f))
        {
            if(ray.transform.tag == "Ladder")
            {
                baseState = BaseState.Climbing;
                pm.Climb();
            }

            if(!Physics.Raycast(transform.position + new Vector3(0,1.5f,0), new Vector3(Direction(), 0, 0), .5f) && ray.transform.tag == "Ledge")
            {
                baseState = BaseState.Climbing;
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
            if (isFalling)
            {
                pc.JumpAttackInit();
                return;
            }

            switch (baseState)
            {
                case BaseState.Running:
                    pc.ComboAttack(Random.Range(1, 4));
                    state = State.Attacking;
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

        else if(combatState == CombatState.Unarmed && ps.hasSword)
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

    void OnTriggerEnter(Collider col)
    {
        if (pc.checkForHit)
        {
            if (col.attachedRigidbody)
            {
                if (col.attachedRigidbody.GetComponent<Enemy>())
                {
                    Enemy e = col.attachedRigidbody.GetComponent<Enemy>();
                    if (!pc.eHits.Contains(e))
                    {
                        pc.eHits.Add(e);
                        e.lives -= 1;
                    }
                }
                if (col.attachedRigidbody.GetComponent<BreakableLootObject>())
                {
                    col.attachedRigidbody.GetComponent<BreakableLootObject>().Break(weapons.endSword.transform.position);
                }
            }
        }
    }
}
