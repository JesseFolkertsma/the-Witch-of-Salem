using UnityEngine;
using System.Collections;

public class Golem : GroundEnemy {
    
    float tempMS;

    bool attack = false;

    public GameObject smashParticle;

    public AudioSource audioS;

    public AudioClip smash;
    public AudioClip getHit;

    void Start()
    {
        GEStart();
        audioS = GetComponent<AudioSource>();
    }

	void Update ()
    {
        GEUpdate();
	}

    public override void Attack()
    {
        if (attack == false)
        {
            anim.SetTrigger("Attack");
            print("Attack");
            tempMS = mSpeed;
            StartCoroutine(WaitForEffect());
            StartCoroutine(WaitForMove());
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        audioS.clip = getHit;
        audioS.Play();
    }

    IEnumerator WaitForEffect()
    {
        yield return new WaitForSeconds(1.7f);
        audioS.clip = smash;
        audioS.Play();
        Collider[] col = Physics.OverlapSphere(transform.position, 4);
        bool hitPlayer = false;
        for (int i = 0; i < col.Length; i++)
        {
            if(col[i].attachedRigidbody != null)
            {
                col[i].attachedRigidbody.AddExplosionForce(100, transform.position, 4);

                if(col[i].attachedRigidbody.GetComponent<PlayerStateMachine>() != null && hitPlayer == false)
                {
                    col[i].attachedRigidbody.GetComponent<PlayerStateMachine>().ps.lives -= 2;
                    hitPlayer = true;
                }
            }
        }

        GameObject tpar = Instantiate(smashParticle, transform.position, transform.rotation * Quaternion.Euler(90, 0, 0)) as GameObject;
        Destroy(tpar, 3);
    }

    IEnumerator WaitForMove()
    {
        mSpeed = 0;
        attack = true;
        yield return new WaitForSeconds(4f);
        mSpeed = tempMS;
        attack = false;
    }
}
