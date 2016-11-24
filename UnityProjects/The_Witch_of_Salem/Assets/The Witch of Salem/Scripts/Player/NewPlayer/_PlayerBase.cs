using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _PlayerBase : MonoBehaviour
{

    public enum BaseState
    {
        CantMove,
        Grounded,
        Climbing,
        Hanging,
        Falling
    };

    public BaseState baseState = BaseState.Grounded;
    public int lives;
    public float movementSpeed, climbSpeed;
    public float jumpHeight;
    public Animator anim;
    public Rigidbody rb;
    public Transform model;
    public _PlayerMouse mouse;
    public LayerMask lm;
    public _PlayerIKHandler ikHandler;

    public float xInput, yInput;
    public bool useRootMovement = true;
    public bool isDead;
    public bool isFalling, canClimbUp;

    public Vector3 holdPos;

    bool canJump;

    public int walkingDirection;
    public int GetMouseDirection
    {
        get
        {
            if (mouse.GetPosition.x - transform.position.x > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        set
        {
            GetMouseDirection = value;
        }
    }

    public void BaseStart()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        model = transform.GetChild(0);
        ikHandler = GetComponentInChildren<_PlayerIKHandler>();
    }

    public virtual void InputHandler()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            ClimbUp();
        }
        //Animator Parameters
        anim.SetFloat("Movement", xInput);
        anim.SetFloat("ClimbingY", yInput);
        anim.SetBool("IsFalling", isFalling);
    }

    public virtual void Checks()
    {
        if(lives < 1)
        {
            Die();
        }

        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f, lm))
        {
            if (baseState != BaseState.Climbing && baseState != BaseState.Hanging)
            {
                isFalling = false;
                baseState = BaseState.Grounded;
            }
        }
        else
        {
            if (baseState != BaseState.Climbing && baseState != BaseState.Hanging)
            {
                isFalling = true;
                baseState = BaseState.Falling;
            }
        }

        Debug.DrawRay(transform.position + Vector3.up, Vector3.right * walkingDirection, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * 1.2f, Vector3.right * walkingDirection, Color.blue);

        RaycastHit climbHit;
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.right * walkingDirection, out climbHit, .3f, lm))
        {
            float offset;
            if(climbHit.transform.position.x > transform.position.x)
            {
                offset = 2;
            }
            else
            {
                offset = -2;
            }

            holdPos = climbHit.collider.ClosestPointOnBounds(transform.position) + Vector3.right * offset;

            if (climbHit.transform.tag == "Ladder" && baseState != BaseState.Climbing)
            {
                baseState = BaseState.Climbing;
            }
            else if (climbHit.transform.tag == "Ledge" && baseState != BaseState.Hanging)
            {
                HangOnLedge();
            }
        }
    }
    
    public virtual void ResetStates()
    {
        baseState = BaseState.Grounded;
    }

    public virtual void Move(float moveSpeed)
    {
        transform.Translate(new Vector3(xInput, 0, 0) * moveSpeed * movementSpeed * Time.fixedDeltaTime);
        if(xInput > 0.1)
        {
            TurnPlayer(true, 1);
        }
        else if (xInput < -.1f)
        {
            TurnPlayer(true, -1);
        }
    }

    public void TurnPlayer(bool smooth, int dir)
    {
        float rot = 0;
        bool doRotate = false;
        if (dir > 0.1)
        {
            rot = 90;
            doRotate = true;
            walkingDirection = 1;
        }
        else if(dir < -.1f)
        {
            rot = -90;
            doRotate = true;
            walkingDirection = -1;
        }

        if (smooth && doRotate)
        {
            model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.Euler(new Vector3(0, rot, 0)), Time.fixedDeltaTime * 6f);
        }
        else if(!smooth && doRotate)
        {
            model.transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0));
        }
    }

    public virtual void Walking()
    {
        if (useRootMovement)
        {
            Move(5);
            canJump = true;
        }
    }

    public virtual void Climbing()
    {
        if (useRootMovement)
        {
            anim.SetLayerWeight(4, 1);
            Vector3 climbVector = new Vector3(0, yInput * climbSpeed / 100, 0);
            transform.Translate(climbVector);
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            if (!Physics.Raycast(transform.position + Vector3.up * 2.5f, Vector3.right * walkingDirection, 1f, lm))
            {
                HangOnLedge();
            }
        }
    }

    public virtual void HangOnLedge()
    {
        rb.useGravity = false;
        baseState = BaseState.Hanging;
        rb.velocity = Vector3.zero;
        print("GO CLIMB");
    }

    public virtual void ClimbUp()
    {
        if (baseState == BaseState.Hanging)
        {
            anim.SetTrigger("ClimbUp");
        }
    }

    public virtual void DropFromClimb()
    {

    }

    public virtual void Falling()
    {
        if (useRootMovement)
        {
            Move(1);
        }
    }

    public virtual void Jump()
    {
        if (canJump && baseState == BaseState.Grounded)
        {
            rb.velocity += Vector3.up * jumpHeight + Vector3.right * xInput * 4;
            anim.SetTrigger("Jump");
        }
    }

    public virtual void Die()
    {
        if (!isDead)
        {
            isDead = true;
        }
    }

    public void StartClimbEvent()
    {
        ActivateRootMotion();
    }

    public void StopClimbEvent()
    {
        DeActivateRootMotion();
        rb.useGravity = true;
        rb.isKinematic = false;
        anim.SetLayerWeight(4, 0);
        ResetStates();
        transform.position = new Vector3(model.transform.position.x, model.transform.position.y, 0);
        model.transform.position = transform.position;
    }

    public void ActivateRootMotion()
    {
        anim.applyRootMotion = true;
    }

    public void DeActivateRootMotion()
    {
        anim.applyRootMotion = false;
    }
}
