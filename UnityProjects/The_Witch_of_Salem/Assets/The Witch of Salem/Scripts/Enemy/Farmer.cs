using UnityEngine;
using System.Collections;

public class Farmer : GroundEnemy {

    bool playerContact = false;
    bool attack = false;
    bool willAttack = false;
    float tempMS;

    AudioSource audioS;
    AudioClip getHit;
    AudioClip attackAudio;

    public Transform fork;
    public Transform forkAttackPos;
    public Transform forkPos;

    Transform currentFork;

    void Awake()
    {
        currentFork = forkPos;
    }

    void Start () {
        GEStart();
	}
	
	// Update is called once per frame
	void Update () {
        GEUpdate();
        fork.transform.rotation = Quaternion.Lerp(fork.transform.rotation, currentFork.transform.rotation, Time.deltaTime * 6);
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
            currentFork = forkAttackPos;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (getHit != null)
        {
            audioS.clip = getHit;
            audioS.Play();
        }
    }

    IEnumerator WaitForEffect()
    {
        yield return new WaitForSeconds(.7f);
        if (attackAudio != null)
        {
            audioS.clip = attackAudio;
            audioS.Play();
        }
        willAttack = true;
        yield return new WaitForSeconds(.5f);
        willAttack = false;
    }

    IEnumerator WaitForMove()
    {
        mSpeed = 0;
        attack = true;
        yield return new WaitForSeconds(3f);
        currentFork = forkPos;
        mSpeed = tempMS;
        attack = false;
    }

    void OnTriggerEnter(Collider col)
    {
        bool hitPlayer = false;
        if (col.tag == "Shield" && col.attachedRigidbody.GetComponent<_PlayerBaseCombat>().combatState == _PlayerBaseCombat.CombatState.Blocking)
        {
            hitPlayer = true;
            willAttack = false;
        }
        if (willAttack)
        {
            if (col.attachedRigidbody)
            {
                col.attachedRigidbody.AddExplosionForce(100, transform.position, 4);

                if (col.attachedRigidbody.GetComponent<_PlayerBaseCombat>() && hitPlayer == false)
                {
                    col.attachedRigidbody.GetComponent<_PlayerBaseCombat>().TakeDamage(1);
                    hitPlayer = true;
                    willAttack = false;
                }
            }
        }
    }
}
