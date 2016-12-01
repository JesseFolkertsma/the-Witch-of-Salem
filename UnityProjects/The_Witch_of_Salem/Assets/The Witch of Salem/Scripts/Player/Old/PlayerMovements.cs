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

    public void Move (float speed)
    {
        CheckForParticle();

        if (psm.Direction() != 0)
        {
            float rot = 0;

            if (psm.Direction() < 0)
            {
                rot = -90;
            }

            if (psm.Direction() > 0)
            {
                rot = 90;
            }

            psm.model.rotation = Quaternion.Lerp(psm.model.rotation, Quaternion.Euler(0, rot, 0), Time.deltaTime * 10f);
        }

        moveSpeed = Mathf.Lerp(moveSpeed, speed, Time.deltaTime * 2f);
        movement *= moveSpeed;
        psm.transform.Translate(new Vector3(movement.x, 0, 0) * psm.movementSpeed * Time.deltaTime);
    }

    void CheckForParticle()
    {
        if (psm.isFalling)
        {
            psm.wParSystem.enableEmission = false;
        }
        else
        {
            psm.wParSystem.enableEmission = true;
        }
    }

    public void Run()
    {
        float runSpeed = 1;
        bool attacking = false;
        if (Input.GetButton("Shift"))
        {
            runSpeed = 1.5f;
        }
        switch (psm.state)
        {
            case PlayerStateMachine.State.Blocking:
                psm.dirMouseBased = true;
                psm.pc.Block();
                runSpeed = .5f;
                break;
            case PlayerStateMachine.State.Aiming:
                psm.dirMouseBased = true;
                psm.pc.DrawArrow();
                runSpeed = .5f;
                break;
            case PlayerStateMachine.State.Rolling:
                runSpeed = 0;
                break;
            case PlayerStateMachine.State.Attacking:
                psm.anim.SetLayerWeight(1, 0);
                attacking = true;
                runSpeed = .1f;
                break;
            case PlayerStateMachine.State.Idle:
                psm.dirMouseBased = false;
                psm.anim.SetLayerWeight(1, 0);
                break;
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
        if (Input.GetButtonDown("Fire1") && !attacking)
        {
            psm.ToAttack();
        }
        if (Input.GetButtonDown("Control"))
        {
            CombatRoll();
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        Move(runSpeed);
    }

    public void Falling()
    {
        float fallSpeed = .5f;
        switch (psm.state)
        {
            case PlayerStateMachine.State.Blocking:
                psm.dirMouseBased = true;
                psm.pc.Block();
                break;
            case PlayerStateMachine.State.Aiming:
                psm.dirMouseBased = true;
                psm.pc.DrawArrow();
                break;
            case PlayerStateMachine.State.JumpAttacking:
                psm.dirMouseBased = false;
                fallSpeed = 0f;
                break;
            case PlayerStateMachine.State.Idle:
                psm.dirMouseBased = false;
                psm.anim.SetLayerWeight(1, 0);
                break;
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
        if (Input.GetButtonDown("Fire1"))
        {
            psm.ToAttack();
        }
        Move(fallSpeed);
    }

    public void ClimbLedge()
    {

        psm.rb.isKinematic = true;
        psm.anim.SetLayerWeight(4, 1);
        if (Input.GetButtonDown("Jump"))
        {
            ClimbUp();
        }

        Vector3 pos;

        if(psm.holdCol.transform.position.x < psm.transform.position.x)
        {
            pos = psm.holdCol.bounds.center + new Vector3(psm.holdCol.bounds.extents.x, psm.holdCol.bounds.extents.y, 0);
            psm.model.transform.rotation = Quaternion.Euler(0, -90, 0);
            psm.transform.position = psm.holdCol.bounds.center + new Vector3(psm.holdCol.bounds.extents.x + .3f, psm.holdCol.bounds.extents.y - 2f, 0);
        }
        else
        {
            pos = psm.holdCol.bounds.center + new Vector3(-psm.holdCol.bounds.extents.x, psm.holdCol.bounds.extents.y, 0);
            psm.model.transform.rotation = Quaternion.Euler(0, 90, 0);
            psm.transform.position = psm.holdCol.bounds.center + new Vector3(-psm.holdCol.bounds.extents.x - .3f, psm.holdCol.bounds.extents.y - 2f, 0);
        }
        //psm.pIK.UseHandIK(pos, true);
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

        if(psm.Direction() == -1 && Input.GetButton("D") && Input.GetButtonDown("Jump"))
        {
            DropClimb();
            psm.rb.velocity += new Vector3(4, 4 ,0);
        }
        if (psm.Direction() == 1 && Input.GetButton("A") && Input.GetButtonDown("Jump"))
        {
            DropClimb();
            psm.rb.velocity += new Vector3(-4, 4, 0);
        }
    }

    public void ClimbUp()
    {
        psm.anim.SetTrigger("ClimbUp");
    }

    public void StartClimbEvent()
    {
        psm.anim.applyRootMotion = true;
        psm.pIK.UseHandIK(Vector3.zero, false);
    }

    public void StopClimbEvent()
    {
        psm.anim.applyRootMotion = false;
        psm.rb.useGravity = true;
        psm.rb.isKinematic = false;
        psm.anim.SetLayerWeight(4, 0);
        psm.baseState = PlayerStateMachine.BaseState.Running;
        psm.transform.position = new Vector3(psm.model.transform.position.x, psm.model.transform.position.y, 0);
        psm.model.transform.position = psm.transform.position;
        psm.pIK.UseHandIK(Vector3.zero, false);
    }

    public void DropClimb()
    {
        psm.rb.isKinematic = false;
        psm.isClimbing = false;
        psm.baseState = PlayerStateMachine.BaseState.Running;
    }

    public void CombatRoll()
    {
        if (canCombatRoll == true)
        {
            if (movement.x < 0)
            {
                psm.rb.AddForce(Vector3.left * 500);
            }
            else
            {
                psm.rb.AddForce(Vector3.right * 500);
            }
            psm.anim.SetTrigger("Roll");
            psm.StartCoroutine(CombatRollCD());
            psm.anim.SetBool("Rolling", true);
            psm.pc.ResetCombatState();
        }
    }

    IEnumerator CombatRollCD()
    {
        canCombatRoll = false;
        yield return new WaitForSeconds(1f);
        psm.ResetPlayer();
        psm.anim.SetBool("Rolling", false);
        yield return new WaitForSeconds(1f);
        canCombatRoll = true;
    }

    public void Jump()
    {
        if (psm.canJump)
        {
            Vector3 jump = new Vector3(movement.x * 3, 7, 0);
            psm.rb.velocity += jump;
            psm.pc.ResetCombatState();
        }
    }
}
