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

    }

    public void SprintAttack()
    {

    }

    public void JumpAttack()
    {

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
    }
}
