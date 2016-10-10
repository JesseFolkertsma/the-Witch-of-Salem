using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour{
    
    public bool agressive;
    public bool inRange;
    public bool canAttack;
    public Transform player;

    public virtual void Move(float movementSpeed, Vector3 target) {}

    public virtual void CheckForPlayer(float detectRange, float attackRange)
    {
        if(Vector3.Distance(transform.position, player.position) < detectRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    public virtual void FollowPlayer() { }

    public virtual void Patolling() { }

    public virtual void Attack() { }
    

}
