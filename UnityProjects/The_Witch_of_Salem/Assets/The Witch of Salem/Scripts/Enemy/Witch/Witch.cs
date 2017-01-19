using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Witch : Boss {

    public enum State
    {
        Idle,
        UsingAbility,
        Dead
    };

    public State state = State.Idle;
    Coroutine routine;

    public List<GameObject> enemies;
    public List<GameObject> enemy;
    public Transform[] enemypos;

    public Transform[] telPositions;
    int currentTelPos = 0;

    public GameObject witchProjectile;
    bool spawned = false;

    public EnemyIKHandler ik;

    //Timers
    public bool willSpawn;
    public bool willTeleport;
    public bool willShoot;
    bool spawnTimer = false;
    bool projectileTimer = false;
    bool teleportTimer = false;
    float spawnTime = 0f;
    float projectileTime = 0f;
    float teleportTime = 0f;
    float maxSpawnT = 10;
    float maxProjectileT = 4;
    float maxTeleportT = 6;
    bool activeWitch = false;

    void Start()
    {
        BossStart();
        lives = 20;
        ik = GetComponentInChildren<EnemyIKHandler>();
        ik.target = player;
    }

    public void Init(List<Transform> e, List<Transform> t)
    {
        enemypos = e.ToArray();
        telPositions = t.ToArray();
        activeWitch = true;
    }

    bool NoSpawns()
    {
        if (enemies.Count == 0)
        {
            return true;
        }
        else {
            for (int i = 0; i < enemies.Count; i++) {
                if (enemies[i] == null)
                {
                    enemies.Remove(enemies[i]);
                }
            }
            if(enemies.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void Update()
    {
        if (activeWitch)
        {
            BossUpdate();
            Timers();
            CheckAbilities();
            LookatPlayer();
        }
    }

    void LookatPlayer()
    {
        Vector3 look = new Vector3(player.position.x, transform.position.y, 0);
        transform.LookAt(look);
    }

    void CheckAbilities()
    {
        if (state == State.Idle)
        {
            if (enemies.Count < 1 && !spawnTimer && willSpawn)
            {
                SpawnEnemies();
            }
            if (!teleportTimer && willTeleport)
            {
                Teleport();
            }
            if (!projectileTimer && willShoot)
            {
                ShootProjectile();
            }
        }
    }

    void Timers()
    {
        if (state == State.Idle)
        {
            if (spawnTimer)
            {
                spawnTime += Time.deltaTime;
                if (spawnTime >= maxSpawnT)
                {
                    enemies.Clear();
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
    }

    void SpawnEnemies()
    {
        if (routine == null)
        {
            if (spawned)
            {
                spawnTimer = true;
            }
            else
            {
                spawned = true;
                routine = StartCoroutine(SpawnE());
            }
        }
    }

    IEnumerator SpawnE()
    {
        anim.SetTrigger("Summon");
        state = State.UsingAbility;

        yield return new WaitForSeconds(1f);

        print("SpawnEnemies");
        spawnTimer = true;
        for (int i = 0; i < enemypos.Length; i++)
        {
            if (enemypos[i] != null)
            {
                int rng = Random.Range(0, enemy.Count);
                GameObject g = Instantiate(enemy[rng], enemypos[i].position, Quaternion.identity) as GameObject;
                enemies.Add(g);
            }
        }

        yield return new WaitForSeconds(.5f);

        state = State.Idle;
        routine = null;
    }

    void Teleport()
    {
        if (routine == null)
        {
            routine = StartCoroutine(TeleportE());
        }
    }

    IEnumerator TeleportE()
    {
        state = State.UsingAbility;
        anim.SetTrigger("Teleport");

        yield return new WaitForSeconds(2f);

        print("Teleport");
        int telpos = currentTelPos + 1;
        if (telpos > telPositions.Length - 1)
        {
            telpos = 0;
        }
        transform.position = telPositions[telpos].position;
        currentTelPos = telpos;
        teleportTimer = true;

        yield return new WaitForSeconds(1f);

        state = State.Idle;
        routine = null;
    }

    void ShootProjectile()
    {
        if (routine == null)
        {
            routine = StartCoroutine(ShootE());
        }
    }

    IEnumerator ShootE()
    {
        state = State.UsingAbility;
        anim.SetTrigger("Shoot");

        yield return new WaitForSeconds(1.5f);
        
        GameObject g = Instantiate(witchProjectile, transform.position, Quaternion.identity) as GameObject;
        g.GetComponent<WitchProjectile>().SetTarget(player);
        projectileTimer = true;

        yield return new WaitForSeconds(.5f);

        state = State.Idle;
        routine = null;
    }

    public override void Die()
    {
        base.Die();
        anim.SetTrigger("Die");
        _GameManager.instance.InstantiateLoad();
        EndBoss();
    }

    public static void EndBoss()
    {
        _GameManager.instance.LoadOtherLevelWithSave(4);
    }
}
