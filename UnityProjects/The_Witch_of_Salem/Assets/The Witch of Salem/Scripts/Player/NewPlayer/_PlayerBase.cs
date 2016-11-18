using UnityEngine;
using System.Collections;

public class _PlayerBase : MonoBehaviour
{

    public enum BaseState
    {
        CantMove,
        Grounded,
        Climbing,
        Falling
    };

    public BaseState baseState = BaseState.Grounded;
    public int lives;
    public float movementSpeed;
    public float jumpHeight;
    public Animator anim;
    public Rigidbody rb;
    public Transform model;
    public _PlayerMouse mouse;
    public LayerMask lm;

    public float xInput;
    public bool useRootMovement = true;
    public bool isDead;
    public bool isFalling;

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
    }

    public virtual void InputHandler()
    {
        xInput = Input.GetAxis("Horizontal");
        //Animator Parameters
        anim.SetFloat("Movement", xInput);
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    public virtual void Checks()
    {
        if(lives < 1)
        {
            Die();
        }

        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f, lm))
        {
            isFalling = false;
        }
        else
        {
            isFalling = true;
        }

        Debug.DrawRay(transform.position + Vector3.up, Vector3.right * walkingDirection, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * 1.2f, Vector3.right * walkingDirection, Color.blue);

        RaycastHit climbHit;
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.right * walkingDirection, out climbHit, 5, lm))
        {
            if(Physics.Raycast(transform.position + Vector3.up * 1.2f, Vector3.right * walkingDirection, 5, lm))
            {
                baseState = BaseState.Climbing;
            }
            else
            {

            }
        }
    }

    public virtual void Move(float moveSpeed)
    {
        transform.Translate(new Vector3(xInput, 0, 0) * moveSpeed * Time.fixedDeltaTime);
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

        }
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
        if (canJump && baseState != BaseState.Falling)
        {
            rb.velocity += Vector3.up * jumpHeight;
        }
    }

    public virtual void Die()
    {
        if (!isDead)
        {
            isDead = true;
        }
    }
}
