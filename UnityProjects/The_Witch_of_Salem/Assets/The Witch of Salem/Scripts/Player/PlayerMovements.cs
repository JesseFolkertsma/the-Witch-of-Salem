using UnityEngine;
using System.Collections;

public class PlayerMovements : MonoBehaviour {

    PlayerStateMachine psm;

    public Vector3 movement;

    public PlayerMovements(PlayerStateMachine p)
    {
        psm = p;
    }

    public void Walk()
    {
        movement *= psm.walkSpeed;
        transform.Translate(new Vector3(movement.x, 0, 0));
    }

    public void Run()
    {
        movement *= psm.runSpeed;
        transform.Translate(new Vector3(movement.x, 0, 0));
    }

    public void Crouch()
    {
        movement *= psm.crouchSpeed;
        transform.Translate(new Vector3(movement.x, 0, 0));
    }

    public void ClimbLedge()
    {
        movement *= psm.climbSpeed;
        transform.Translate(new Vector3(0, movement.y, 0));
    }

    public void ClimbLadder()
    {
        movement *= psm.climbSpeed;
        transform.Translate(new Vector3(0, movement.y, 0));
    }

    public void CombatRoll()
    {

    }

    public void Jump()
    {

    }
}
