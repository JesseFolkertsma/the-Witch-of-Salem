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

    public float mSpeed;
    public float detectionRange;
    public float attackRange;

    bool inRange;
    bool canAttack;

    public virtual void GEStart()
    {
        enemyModel = transform.GetChild(0);
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
        Move(mSpeed, waypoints[currentwp].position);
        if(inRange == true)
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

    public virtual void Attack() { }
}
