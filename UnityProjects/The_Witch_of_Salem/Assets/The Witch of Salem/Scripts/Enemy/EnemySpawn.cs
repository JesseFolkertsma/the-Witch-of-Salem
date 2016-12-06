using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemySpawn {

    public GameObject enemy;
    public Transform enemyPosition;
    Enemy enemyData;
    EnemySpawnEvent spawn;

    public float waitSec;
    public int waitForDead;
    public bool isDead = false;

    public void Spawn(EnemySpawnEvent _spawn)
    {
        GameObject g = MonoBehaviour.Instantiate(enemy, enemyPosition.position, Quaternion.identity) as GameObject;
        enemyData = g.GetComponent<Enemy>();
        enemyData.spawn = this;
        spawn = _spawn;
    }

    public void Die()
    {
        isDead = true;
        spawn.CheckForEnd();
    }
}
