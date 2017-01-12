using UnityEngine;
using System.Collections;

public class Raven : Enemy {

    public enum AttackState
    {
        Flapping,
        Thrusting
    };

    public AttackState attackState = AttackState.Flapping;

    Vector3 playerPos;
    Animator anim;

    public float flapStrenght;
    public float flySpeed;
    public float flapCD;
    public float thrustCD = 5f;
    public float thrustForce = 100f;
    bool canFlap = true;
    bool canThrust = true;
    Rigidbody rb;

    [SerializeField]
    GameObject ragdoll;

    void Start()
    {
        GEStart();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(ThrustCD());
        anim = GetComponentInChildren<Animator>();
    }

	void Update()
    {
        Flap();
        Thrust();
        Thrusting();

        data.lives = lives;

        if (lives < 1)
        {
            if (!isDead)
            {
                Die();
            }
        }
    }

    void Thrust()
    {
        if (canThrust)
        {
            anim.SetBool("Thrusting", true);
            StartCoroutine(ThrustCD());
            attackState = AttackState.Thrusting;
            playerPos = player.transform.position;
            rb.AddForce((player.transform.position - transform.position).normalized * thrustForce);
        }
    }

    void Thrusting()
    {
        if (attackState == AttackState.Thrusting)
        {
            transform.rotation = Quaternion.LookRotation(playerPos);
        }
    }
    
    IEnumerator ThrustCD()
    {
        thrustCD = Random.Range(3, 7);
        canThrust = false;
        yield return new WaitForSeconds(thrustCD);
        canThrust = true;
    }

    void Flap()
    {
        if (transform.position.y - player.position.y < 5)
        {
            flapCD = .3f;
        }
        else
        {
            flapCD = 1f;
        }
        if (attackState == AttackState.Flapping)
        {
            anim.SetBool("Thrusting", false);
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            if (canFlap)
            {
                StartCoroutine(FlapCD());

                if (CheckTargetDir(player.position) == TargetDirection.Right)
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    rb.velocity = new Vector3(5, flapStrenght, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                    rb.velocity = new Vector3(-5, flapStrenght, 0);
                }
            }
        }
    }

    IEnumerator FlapCD()
    {
        canFlap = false;
        yield return new WaitForSeconds(flapCD);
        canFlap = true;
    }

    public override void Die()
    {
        base.Die();
        GameObject r = Instantiate(ragdoll, transform.position, transform.rotation) as GameObject;
        r.GetComponentInChildren<Rigidbody>().AddForce(rb.velocity * 100);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        attackState = AttackState.Flapping;
        if (col.collider.attachedRigidbody)
        {
            if (col.collider.attachedRigidbody.GetComponent<_Player>())
            {
                col.collider.attachedRigidbody.GetComponent<_Player>().TakeDamage(1);
            }
        }
    }
}
