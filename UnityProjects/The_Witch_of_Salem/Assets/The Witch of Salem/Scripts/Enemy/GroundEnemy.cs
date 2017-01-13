using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundEnemy : Enemy {
        
    Transform enemyModel;
    public Animator anim;

    public float mSpeed;
    public float attackRange;

    public bool walking;
    
    public bool canAttack = false;

    public GameObject ragdoll;

    public override void GEStart()
    {
        base.GEStart();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        enemyModel = transform.GetChild(0);
        canAttack = true;
    }

    public virtual void GEUpdate()
    {
        CheckForPlayer(attackRange);

        FollowPlayer();

        anim.SetBool("IsWalking", walking);
        if(lives < 1)
        {
            Die();
        }
        data.lives = lives;
    }

    public virtual void CheckForPlayer(float attackRange)
    {
        if (Vector3.Distance(transform.position , player.position) < attackRange)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    public virtual void Move(float movementSpeed, Vector3 target)
    {
        if (movementSpeed > 0.1f && !Physics.Raycast(transform.position + enemyModel.transform.forward / 2, enemyModel.transform.forward, 2))
            walking = true;
        else
        {
            walking = false;
            movementSpeed = 0;
        }

        targetDir = CheckTargetDir(target);
        float rot = 0;
        Vector3 dir = new Vector3(0, 0, 0);

        if (targetDir == TargetDirection.Left)
        {
            rot = -90f;
            dir.x = -1;
        }
        else
        {
            rot = 90f;
            dir.x = 1;
        }

        enemyModel.transform.rotation = Quaternion.Lerp(enemyModel.transform.rotation, Quaternion.Euler(0, rot, 0), Time.deltaTime * 5f);
        transform.Translate(dir * movementSpeed * Time.deltaTime);
    }

    public virtual void FollowPlayer()
    {
        Move(mSpeed, player.position);

        if(canAttack == true)
        {
            Attack();
        }
    }

    public override void Die()
    {
        print("Death" + gameObject.name);
        if (isDead == false)
        {
            Instantiate(ragdoll, transform.position, enemyModel.rotation);
            Destroy(gameObject);
        }
        base.Die();
    }

    public virtual void Attack() { }
}
