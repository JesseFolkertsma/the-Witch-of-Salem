using UnityEngine;
using System.Collections;

public class Raven : Enemy {

    public float flapStrenght;
    public float flySpeed;
    public float flapCD;
    public float thrustCD = 5f;
    bool canFlap = true;
    bool canThrust = true;
    Rigidbody rb;

    [SerializeField]
    GameObject ragdoll;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(ThrustCD());
    }

	void Update()
    {
        Flap();

        transform.rotation = Quaternion.LookRotation(rb.velocity);

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
            StartCoroutine(ThrustCD());
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

    IEnumerator FlapCD()
    {
        canFlap = false;
        yield return new WaitForSeconds(flapCD);
        canFlap = true;
    }

    public override void Die()
    {
        base.Die();
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
