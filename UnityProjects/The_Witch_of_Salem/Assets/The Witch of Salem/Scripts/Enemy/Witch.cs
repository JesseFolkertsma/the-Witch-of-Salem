using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Witch : Boss {

    public List<GameObject> enemies;
    public GameObject enemy;
    public Transform[] enemypos;

    public Transform[] telPositions;
    int currentTelPos = 0;

    public GameObject witchProjectile;
    bool spawned = false;

    //Timers
    public bool spawnTimer = false;
    bool projectileTimer = false;
    bool teleportTimer = false;
    public float spawnTime = 0f;
    float projectileTime = 0f;
    float teleportTime = 0f;
    public float maxSpawnT = 10;
    float maxProjectileT = 4;
    float maxTeleportT = 6;

    void Start()
    {
        BossStart();
        lives = 20;
    }

    void Update()
    {
        Timers();
        if(enemies.Count < 1 && !spawnTimer)
        {
            SpawnEnemies();
        }
        if(!teleportTimer)
        {
            Teleport();
        }
        if (!projectileTimer)
        {
            ShootProjectile();
        }

        if (spawned)
        {
            if(enemies[0] == null)
            {
                spawnTimer = true;
            }
        }
    }

    void Timers()
    {
        if (spawnTimer)
        {
            spawnTime += Time.deltaTime;
            if(spawnTime >= maxSpawnT)
            {
                spawnTimer = false;
                spawnTime = 0f;
                spawned = false;
            }
        }
        if (projectileTimer)
        {
            projectileTime += Time.deltaTime;
            if (projectileTime >= maxProjectileT)
            {
                projectileTimer = false;
                projectileTime = 0f;
            }
        }
        if (teleportTimer)
        {
            teleportTime += Time.deltaTime;
            if (teleportTime >= maxTeleportT)
            {
                teleportTimer = false;
                teleportTime = 0f;
            }
        }
    }

    void SpawnEnemies()
    {
        if (spawned)
        {
            spawnTimer = true;
        }
        else
        {
            spawned = true;
            print("SpawnEnemies");
            //spawnTimer = true;
            for (int i = 0; i < enemypos.Length; i++)
            {
                if (enemypos[i] != null)
                {
                    GameObject g = Instantiate(enemy, enemypos[i].position, Quaternion.identity) as GameObject;
                    enemies.Add(g);
                }
            }
        }
    }

    void Teleport()
    {
        print("Teleport");
        int telpos = currentTelPos + 1;
        if (telpos > telPositions.Length - 1)
        {
            telpos = 0;
        }
        transform.position = telPositions[telpos].position;
        currentTelPos = telpos;
        teleportTimer = true;
    }

    void ShootProjectile()
    {
        Instantiate(witchProjectile, transform.position, Quaternion.identity);
        projectileTimer = true;
    }
}
