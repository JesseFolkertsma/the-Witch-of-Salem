using UnityEngine;
using System.Collections;

public class Golem : GroundEnemy {
    
    float tempMS;

    bool attack = false;

    public GameObject smashParticle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GEStart();
    }

	void Update () {
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

    IEnumerator WaitForEffect()
    {
        yield return new WaitForSeconds(1.7f);

        GameObject tpar = Instantiate(smashParticle, transform.position, transform.rotation * Quaternion.Euler(90, 0, 0)) as GameObject;
        Destroy(tpar, 3);
    }

    IEnumerator WaitForMove()
    {
        mSpeed = 0;
        attack = true;
        yield return new WaitForSeconds(3f);
        mSpeed = tempMS;
        attack = false;
    }
}
