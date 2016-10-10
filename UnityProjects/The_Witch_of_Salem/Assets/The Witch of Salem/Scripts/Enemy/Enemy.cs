using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour{
    
    public bool agressive;
    public bool inRange;
    public Transform player;

    public virtual void Move(float movementSpeed)
    {

    }

    public virtual void CheckForPlayer(float detectRange)
    {
        if(Vector3.Distance(transform.position, player.position) < detectRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
    }

    public virtual void FollowPlayer() { }

    public virtual void Attack() { }
}
