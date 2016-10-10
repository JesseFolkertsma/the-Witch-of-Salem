using UnityEngine;
using System.Collections;

public class Golem : GroundEnemy {

    public Animator anim;

    float tempMS;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	void Update () {
        GEUpdate();
	}

    public override void Attack()
    {
        anim.SetTrigger("Attack");
        print("Attack");
        tempMS = mSpeed;
        StartCoroutine(IEAttack());
    }

    IEnumerator IEAttack()
    {
        mSpeed = 0;
        yield return new WaitForSeconds(3f);
        mSpeed = tempMS;
    }
}
