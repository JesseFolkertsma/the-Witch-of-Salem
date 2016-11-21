using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : PlayerComponent {

    PlayerStateMachine psm;
    bool canShoot = true;
    bool lookAt;
    float str = 0;

    bool jumpAttackCD;
    
    bool waitForAttack;
    public bool checkForHit;
    public bool attacking;

    public int currentCombo;
    public float waitForAttackTime;
    Coroutine wait;

    public List<Enemy> eHits = new List<Enemy>();

    public Animator bowAnim;
    
    public PlayerCombat (PlayerStateMachine p)
    {
        psm = p;
    }

    public void CombatUpdate()
    {
        if (Input.GetButton("Fire1") && waitForAttack)
        {
            waitForAttack = false;
            Debug.Log("HEY");
            ComboAttack(currentCombo + 1);
            psm.StopCoroutine(wait);
        }
    }

    public void ComboAttack(int combo)
    {
        if (combo == 4)
        {
            combo = 1;
        }
        currentCombo = combo;
        attacking = true;
        waitForAttack = false;
        psm.anim.SetTrigger("Attack");
    }

    public void CheckForHit()
    {
        checkForHit = true;
        Debug.Log("CHecking for hits");
        psm.audioS.clip = psm.swing;
        psm.audioS.Play();
    }

    public void StopCheckForHit()
    {
        checkForHit = false;
        eHits.Clear();
        Debug.Log("Stop checking for hits");
    }

    public void ActivateWait()
    {
        wait = psm.StartCoroutine(WaitforAttack());
    }

    IEnumerator WaitforAttack()
    {
        waitForAttack = true;
        yield return new WaitForSeconds(.4f);
        waitForAttack = false;
        ExitComboLoop();
    }

    void ExitComboLoop()
    {
        Debug.Log("Stop waiting");
        psm.state = PlayerStateMachine.State.Idle;
        attacking = false;
        waitForAttack = false;
    }

    public void Stagger()
    {
        StopCheckForHit();
        ExitComboLoop();
    }

    public void SprintAttack()
    {

    }

    public void JumpAttackInit()
    {
        if (psm.ps.hasJumpAttack == true && jumpAttackCD == false)
        {
            psm.rb.velocity = new Vector3(0, -10, 0);
            psm.jumpAttack = true;
            psm.anim.SetTrigger("JumpAttack");
        }
    }

    public void JumpAttack()
    {
        if(psm.isFalling == false)
        {
            Collider[] hits = Physics.OverlapSphere(psm.transform.position, 5f);
            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(new Enemy());
            
            for(int i = 0; i < hits.Length; i++)
            {
                if(hits[i].GetComponent<BreakableLootObject>() != null)
                {
                    hits[i].GetComponent<BreakableLootObject>().Break(psm.transform.position);
                }

                if (hits[i].attachedRigidbody != null)
                {
                    if (hits[i].attachedRigidbody.GetComponent<Enemy>() != null)
                    {
                        if (enemies[enemies.Count - 1] != hits[i].attachedRigidbody.GetComponent<Enemy>())
                        {
                            hits[i].attachedRigidbody.GetComponent<Enemy>().lives -= 2;
                            enemies.Add(hits[i].attachedRigidbody.GetComponent<Enemy>());
                        }
                    }
                }

                if (hits[i].GetComponent<Rigidbody>() != null)
                {
                    hits[i].GetComponent<Rigidbody>().AddExplosionForce(1000, psm.transform.position, 5f, 10);
                }
            }
            psm.jumpAttack = false;
            GameObject tpar  = MonoBehaviour.Instantiate(psm.smashParticles, psm.transform.position + new Vector3(0, .7f, 0), psm.transform.rotation * Quaternion.Euler(90,0,0)) as GameObject;
            MonoBehaviour.Destroy(tpar, 3);
            psm.StartCoroutine(JumpAttackCD());
        }
    }

    IEnumerator JumpAttackCD()
    {
        jumpAttackCD = true;
        yield return new WaitForSeconds(5);
        jumpAttackCD = false;
    }

    public void PierceAttack()
    {

    }

    public void DrawArrow()
    {
        psm.anim.SetBool("AimBow", true);
        if (psm.ps.arrows > 0)
        {
            if (Input.GetButton("Fire1"))
            {
                str = Mathf.Lerp(str, 20, Time.deltaTime * 1f);
                Debug.Log(str);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                ShootArrow(str);
                str = 0;
            }
        }
        psm.anim.SetFloat("DrawStrenght", str);
        bowAnim.SetFloat("Blend", str);

        LookAtMouse(1);

        if (Input.GetButtonUp("Fire2"))
        {
            psm.state = PlayerStateMachine.State.Idle;
            psm.anim.SetBool("AimBow", false);
        }
    }

    public void ResetCombatState()
    {
        psm.state = PlayerStateMachine.State.Idle;
    }

    public void ShootArrow(float strenght)
    {
        psm.ps.arrows--;
        Vector3 dir = psm.mouse.position - psm.transform.position;
        dir.Normalize();
        GameObject arrow = GameObject.Instantiate(psm.arrow, psm.weapons.bow.transform.position + dir, Quaternion.identity) as GameObject;
        arrow.GetComponent<Arrow>().Shoot(dir, strenght);
        Debug.Log("Shoot");
    }

    public void Block()
    {
        psm.anim.SetLayerWeight(1, 1);
        LookAtMouse(.3f);
        if (Input.GetButtonUp("Fire2"))
        {
            psm.state = PlayerStateMachine.State.Idle;
            psm.anim.SetLayerWeight(1, 0);
        }
    }

    public void LookAtMouse(float str)
    {
        psm.pIK.UseIK(psm.mouse, str);
    }

}