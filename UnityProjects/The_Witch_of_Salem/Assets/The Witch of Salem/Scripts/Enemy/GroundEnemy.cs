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
    public int currentwp;

    public Transform target;
    public Transform head;

    public float mSpeed;
    public float dRange;
    public float aRange;

    public float waitTimer;

    public virtual void GEUpdate()
    {
        CheckForPlayer(dRange, aRange);
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

    public override void Move(float movementSpeed, Vector3 target)
    {
        transform.LookAt(new Vector3(target.x, transform.position.y, 0));
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    public override void Patolling()
    {
        base.Patolling();
        Move(mSpeed, waypoints[currentwp].position);
        if(inRange == true)
        {
            state = EnemyState.Following;
        }
    }

    public override void FollowPlayer()
    {
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
}
