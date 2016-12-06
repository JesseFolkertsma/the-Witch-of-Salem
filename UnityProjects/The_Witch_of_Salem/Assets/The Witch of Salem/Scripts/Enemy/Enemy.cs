using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public enum TargetDirection
    {
        Left,
        Right
    }

    public TargetDirection targetDir;

    public bool active;

    public _EnemyUIData data;
    [HideInInspector]
    public EnemySpawn spawn;

    public int lives;
    public float movementSpeed;
    public bool isAgressive;
    public bool isDead = false;

    public Transform player;

    public virtual TargetDirection CheckTargetDir(Vector3 target)
    {
        if(transform.position.x < target.x)
        {
            return TargetDirection.Right;
        }
        else
        {
            return TargetDirection.Left;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        lives -= damage;
    }
    
    public virtual void Die()
    {
        isDead = true;
        if (spawn != null)
        {
            spawn.Die();
        }
    }
}
