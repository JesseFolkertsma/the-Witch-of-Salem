using UnityEngine;
using System.Collections;

public class PlayerCombat : PlayerComponent {

    PlayerStateMachine psm;
    bool canShoot = true;
    float str = 0;
    
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
        psm.rb.velocity = new Vector3(0, -10, 0);
        psm.jumpAttack = true;
    }

    public void JumpAttack()
    {
        if(psm.isFalling == false)
        {
            Collider[] hits = Physics.OverlapSphere(psm.transform.position, 5f);
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
                        hits[i].attachedRigidbody.GetComponent<Enemy>().health -= 50;
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
        }
    }

    public void PierceAttack()
    {

    }

    public void DrawArrow()
    {
        LookAtMouse();
        psm.pm.Move(.5f, true);
        if (Input.GetButtonUp("Fire2"))
        {
            psm.state = PlayerStateMachine.State.Walking;
        }
        
        if (Input.GetButton("Fire1"))
        {
            str = Mathf.Lerp(str, 20, .02f);
            Debug.Log(str);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            ShootArrow(str);
            str = 0;
        }
    }

    public void ShootArrow(float strenght)
    {
        Vector3 dir = psm.mouse.position - psm.transform.position;
        dir.Normalize();
        GameObject arrow = GameObject.Instantiate(psm.arrow, psm.bow.transform.position + dir, Quaternion.identity) as GameObject;
        arrow.GetComponent<Arrow>().Shoot(dir, strenght);
        Debug.Log("Shoot");
    }

    public void Block()
    {
        LookAtMouse();
        psm.pm.Move(1, true);
        psm.shield.SetActive(true);
        if (Input.GetButtonUp("Fire2"))
        {
            psm.state = PlayerStateMachine.State.Walking;
            psm.shield.SetActive(false);
        }
    }

    public void LookAtMouse()
    {
        psm.backBone.LookAt(psm.mouse.position);

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