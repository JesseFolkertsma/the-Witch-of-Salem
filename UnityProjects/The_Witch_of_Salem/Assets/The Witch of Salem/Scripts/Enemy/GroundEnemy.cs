using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundEnemy : Enemy {

    public enum EnemyState
    {
        Patrolling,
        Following
    };

    public EnemyState state;

    public List<Transform> waypoints;
    
    [HideInInspector]
    public int currentwp;
    
    public Transform head;
    Transform enemyModel;
    public Animator anim;

    public float mSpeed;
    public float detectionRange;
    public float attackRange;

    public bool walking;

    public bool randomStops;
    public Vector2 minMaxWalkTime;
    public Vector2 minMaxStopTime;
    float walkTime;
    float stopTime;
    float walkTimer;
    float stopTimer;

    bool inRange;
    bool canAttack;

    public GameObject ragdoll;

    public virtual void GEStart()
    {
        anim = GetComponentInChildren<Animator>();
        enemyModel = transform.GetChild(0);
        walkTime = Random.Range(minMaxWalkTime.x, minMaxWalkTime.y);
        stopTime = Random.Range(minMaxStopTime.x, minMaxStopTime.y);
    }

    public virtual void GEUpdate()
    {
        CheckForPlayer(detectionRange, attackRange);
        switch (state)
        {
            case EnemyState.Patrolling:
                Patolling();
                break;
            case EnemyState.Following:
                FollowPlayer();
                break;
        }
        anim.SetBool("IsWalking", walking);
    }

    public virtual void CheckForPlayer(float detectRange, float attackRange)
    {
        if (Vector3.Distance(transform.position, player.position) < detectRange)
        {
            inRange = true;
        }

        if (Vector3.Distance(transform.position, player.position) < attackRange)
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
        if (movementSpeed > 0.1f)
            walking = true;
        else
            walking = false;

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

        enemyModel.transform.rotation = Quaternion.Lerp(enemyModel.transform.rotation, Quaternion.Euler(0, rot, 0), .1f);
        transform.Translate(dir * movementSpeed * Time.deltaTime);
    }

    public virtual void Patolling()
    {

        float speed = mSpeed;

        if(randomStops == true)
        {
            walkTimer += 1 * Time.deltaTime;
            if(walkTimer >= walkTime)
            {
                speed = 0;
                stopTimer += 1 * Time.deltaTime;
                if(stopTimer >= stopTime)
                {
                    walkTime = Random.Range(minMaxWalkTime.x, minMaxWalkTime.y);
                    stopTime = Random.Range(minMaxStopTime.x, minMaxStopTime.y);
                    walkTimer = 0;
                    stopTimer = 0;
                }
            }
        }

        Move(speed, waypoints[currentwp].position);

        if (inRange == true)
        {
            state = EnemyState.Following;
        }
    }

    public virtual void FollowPlayer()
    {
        print("OMNOMNOMNOMNOM");
        Move(mSpeed, player.position);
        head.LookAt(player);
        if(inRange == false)
        {
            state = EnemyState.Patrolling;
        }
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
