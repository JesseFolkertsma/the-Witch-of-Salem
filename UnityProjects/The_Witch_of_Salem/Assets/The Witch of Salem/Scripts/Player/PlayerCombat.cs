using UnityEngine;
using System.Collections;

public class PlayerCombat : PlayerComponent {

    PlayerStateMachine psm;
    
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

    public void JumpAttack()
    {
        psm.rb.velocity = new Vector3(0, -10, 0);
        while(psm.isFalling == true)
        {
            if(psm.isFalling == false)
            {
                Debug.Log("BOOOOM");
                break;
            }
        }
    }

    public void PierceAttack()
    {

    }

    public void DrawArrow()
    {

    }

    public void ShootArrow()
    {

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
