using UnityEngine;
using System.Collections;

public class PlayerMovements : PlayerComponent {

    PlayerStateMachine psm;

    public Vector3 movement;
    float moveSpeed = 1f;

    bool canCombatRoll = true;

    public PlayerMovements(PlayerStateMachine p)
    {
        psm = p;
    }

    public void Move (float speed, bool combat)
    {
        if (combat == false)
        {
            if (movement.x < 0)
            {
                psm.dir.x = -1;
            }
            else if (movement.x > 0)
            {
                psm.dir.x = 1;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                if (psm.combatState == PlayerStateMachine.CombatState.Melee)
                {
                    psm.state = PlayerStateMachine.State.Blocking;
                }
                if (psm.combatState == PlayerStateMachine.CombatState.Ranged)
                {
                    psm.state = PlayerStateMachine.State.Aiming;
                }
            }

            if (psm.isFalling == false)
            {
                if (Input.GetButtonDown("Control"))
                {
                    CombatRoll();
                }
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                psm.ToAttack();
            }
        }

        float rot = 0;

        if(psm.dir.x < 0)
        {
            rot = -90;
        }

        if (psm.dir.x > 0)
        {
            rot = 90;
        }

        psm.model.rotation = Quaternion.Lerp(psm.model.rotation, Quaternion.Euler(0, rot, 0), Time.deltaTime * 5f);

        if (psm.isFalling == false)
        {
            psm.wParSystem.enableEmission = true;
        }
        else
        {
            psm.wParSystem.enableEmission = false;
        }

        moveSpeed = Mathf.Lerp(moveSpeed, speed, Time.deltaTime * 2f);
        movement *= moveSpeed;
        psm.transform.Translate(new Vector3(movement.x, 0, 0) * psm.movementSpeed * Time.deltaTime);
    }

    public void Walk()
    {
        Move(1, false);
        
        if (Input.GetButton("Shift"))
        {
            psm.state = PlayerStateMachine.State.Running;
        }
        else if (Input.GetButtonDown("S"))
        {
            psm.state = PlayerStateMachine.State.Crouching;
        }
    }

    public void Run()
    {
        Move(2, false);

        if (Input.GetButtonUp("Shift"))
        {
            psm.state = PlayerStateMachine.State.Walking;
        }
        else if (Input.GetButtonDown("S"))
        {
            psm.state = PlayerStateMachine.State.Crouching;
        }
    }

    public void Crouch()
    {
        Move(.5f, false);

        if (Input.GetButtonDown("Shift"))
        {
            psm.state = PlayerStateMachine.State.Running;
        }
        else if (Input.GetButtonUp("S"))
        {
            psm.state = PlayerStateMachine.State.Walking;
        }
    }

    public void Falling()
    {
        Move(.5f, false);
    }

    public void ClimbLedge()
    {
        if (psm.isClimbing == false)
        {
            psm.rb.isKinematic = true;
            psm.isClimbing = true;
        }

        moveSpeed = Mathf.Lerp(moveSpeed, 1, Time.deltaTime * 2f);
        movement *= moveSpeed;
        psm.transform.Translate(new Vector3(0, movement.y, 0) * psm.climbSpeed * Time.deltaTime);
    }

    public void Climb()
    {
        if(psm.isClimbing == false)
        {
            psm.rb.isKinematic = true;
            psm.isClimbing = true;
        }

        moveSpeed = Mathf.Lerp(moveSpeed, 1, Time.deltaTime * 2f);
        movement *= moveSpeed;
        psm.transform.Translate(new Vector3(0, movement.y, 0) * psm.climbSpeed * Time.deltaTime);

        if(psm.dir.x == -1 && Input.GetButton("D") && Input.GetButtonDown("Jump"))
        {
            DropClimb();
            psm.rb.velocity += new Vector3(4, 4 ,0);
        }
        if (psm.dir.x == 1 && Input.GetButton("A") && Input.GetButtonDown("Jump"))
        {
            DropClimb();
            psm.rb.velocity += new Vector3(-4, 4, 0);
        }
    }

    public void ClimbUp()
    {
        psm.rb.isKinematic = false;
        psm.isClimbing = false;
        psm.transform.position += new Vector3(psm.dir.x, 1, 0);
        psm.state = PlayerStateMachine.State.Walking;
    }

    public void DropClimb()
    {
        psm.rb.isKinematic = false;
        psm.isClimbing = false;
        psm.state = PlayerStateMachine.State.Walking;
    }

    public void CombatRoll()
    {
        if (canCombatRoll == true)
        {
            psm.rb.velocity += psm.dir * 10;
            psm.StartCoroutine(CombatRollCD());
        }
    }

    IEnumerator CombatRollCD()
    {
        canCombatRoll = false;
        yield return new WaitForSeconds(2f);
        canCombatRoll = true;
    }

    public void Jump()
    {
        Vector3 jump = new Vector3(movement.x * 3, 7, 0);
        psm.rb.velocity += jump;
    }
}
