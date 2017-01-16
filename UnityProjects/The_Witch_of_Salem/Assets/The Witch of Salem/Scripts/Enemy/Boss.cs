using UnityEngine;
using System.Collections;

public class Boss : Enemy {

    public Animator anim;
    public Rigidbody rb;
    public GameObject ragdoll;
    
    public void BossStart()
    {
        GEStart();
        player = FindObjectOfType<_PlayerBase>().transform;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void BossUpdate()
    {
        data.lives = lives;
        if(lives < 1)
        {
            Die();
        }
    }

    public override void Die()
    {
        if (!isDead)
        {
            base.Die();

            if (ragdoll != null)
            {
                Instantiate(ragdoll, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogWarning("Boss has no ragdoll!");
            }

            Destroy(gameObject);
        }
    }
}
