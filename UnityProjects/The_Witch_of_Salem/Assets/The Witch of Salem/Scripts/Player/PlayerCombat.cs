using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : PlayerComponent {

    PlayerStateMachine psm;
    bool canShoot = true;
    bool lookAt;
    float str = 0;

    bool jumpAttackCD;
    
    public PlayerCombat (PlayerStateMachine p)
    {
        psm = p;
    }

	public void BasicAttack()
    {
        RaycastHit hit;
        if(Physics.Raycast(psm.transform.position, psm.dir, out hit, 3f))
        {
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(psm.dir * 100);
            }
        }
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
            GameObject tpar  = MonoBehaviour.Instantiate(psm.smashParticles, psm.transform.position - new Vector3(0, .7f, 0), psm.transform.rotation * Quaternion.Euler(90,0,0)) as GameObject;
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

        LookAtMouse();
        psm.pm.Move(.5f, true);

        if (Input.GetButtonUp("Fire2"))
        {
            psm.state = PlayerStateMachine.State.Walking;
        }
    }

    public void ShootArrow(float strenght)
    {
        psm.ps.arrows--;
        Vector3 dir = psm.mouse.position - psm.transform.position;
        dir.Normalize();
        GameObject arrow = GameObject.Instantiate(psm.arrow, psm.bow.transform.position + dir, Quaternion.identity) as GameObject;
        arrow.GetComponent<Arrow>().Shoot(dir, strenght);
        Debug.Log("Shoot");
    }

    public void Block()
    {
        psm.anim.SetLayerWeight(1, 1);
        LookAtMouse();
        psm.pm.Move(.5f, true);
        if (Input.GetButtonUp("Fire2"))
        {
            psm.state = PlayerStateMachine.State.Walking;
            psm.shield.SetActive(false);
            psm.anim.SetLayerWeight(1, 0);
        }
    }

    public void LookAtMouse()
    {
        psm.pIK.UseIK(psm.mouse);
        if (psm.mouse.position.x > psm.transform.position.x)
        {
            psm.dir.x = 1;
        }
        else
        {
            psm.dir.x = -1;
        }
    }
}