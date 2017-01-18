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

    public int lives;
    public int maxLives = 5;
    public int apples;

    public BaseState baseState = BaseState.Grounded;
    public float movementSpeed, climbSpeed;
    public float jumpHeight;
    public Animator anim;
    public Rigidbody rb;
    public Transform model;
    public _PlayerMouse mouse;
    public LayerMask lm;
    public _PlayerIKHandler ikHandler;
    public CapsuleCollider col;
    public float standardColSize = 1.8f;

    public Collider climbingOnObject;

    public float xInput, yInput;
    public bool useRootMovement = true;
    public bool isDead;
    bool isFalling, canClimbUp;

    public GameObject ragdoll;

    bool canJump;

    public delegate void OnDeath();
    static public OnDeath OnDeathEvent;

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

    public virtual void BaseStart()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        model = transform.GetChild(0);
        ikHandler = GetComponentInChildren<_PlayerIKHandler>();
        //mouse = GameObject.FindObjectOfType<_PlayerMouse>();
        col = GetComponent<CapsuleCollider>();
        standardColSize = col.height;
        _GameManager.instance.PlayerDeath(false);
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
        if (Input.GetButtonDown("Q"))
        {
            Heal(1, true);
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

        Debug.DrawRay(transform.position + Vector3.up * 1.5f, Vector3.right * walkingDirection, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * 2f, Vector3.right * walkingDirection, Color.blue);

        RaycastHit climbHit;
        if(Physics.Raycast(transform.position + Vector3.up * 1.5f, Vector3.right * walkingDirection, out climbHit, .4f, lm))
        {
            if (climbHit.transform.tag == "Ladder" && baseState != BaseState.Climbing && baseState != BaseState.Hanging) 
            {
                baseState = BaseState.Climbing;
                climbingOnObject = climbHit.collider;
            }
            else if (climbHit.transform.tag == "Ledge" && baseState != BaseState.Hanging)
            {
                climbingOnObject = climbHit.collider;
                HangOnLedge();
            }
        }
    }
    
    public virtual void ResetStates()
    {
        baseState = BaseState.Grounded;
        //col.height = standardColSize;
    }

    public void Heal(int _lives, bool useApple)
    {
        if (lives < maxLives)
        {
            if (apples > 0 && useApple)
            {
                lives += _lives;
                apples--;
            }
            if (!useApple)
            {
                lives += _lives;
            }
        }
        if(lives > maxLives)
        {
            lives = maxLives;
        }
        _UIManager.instance.UpdateUI();
    }

    public virtual void Move(float moveSpeed, bool turnToMouse)
    {
        //float velocity = xInput * moveSpeed * Time.deltaTime;
        //rb.MovePosition(rb.position + Vector3.right * velocity);
        transform.Translate(new Vector3(xInput, 0, 0) * moveSpeed * movementSpeed * Time.deltaTime);
        
        if (turnToMouse)
        {
            TurnPlayer(true, GetMouseDirection);
        }
        else
        {
            if (xInput > 0.1)
            {
                TurnPlayer(true, 1);
            }
            else if (xInput < -.1f)
            {
                TurnPlayer(true, -1);
            }
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
            Move(5, false);
            canJump = true;
        }
    }

    public virtual void Climbing()
    {
        if (useRootMovement)
        {
            anim.SetBool("IsHanging", false);
            float offset = .3f;
            float wallPosition = climbingOnObject.bounds.extents.x + offset;
            float newPlayerX = 0f;

            if (climbingOnObject.transform.position.x > transform.position.x)
            {
                newPlayerX = climbingOnObject.transform.position.x - wallPosition;
            }
            else
            {
                newPlayerX = climbingOnObject.transform.position.x + wallPosition;
            }

            transform.position = new Vector3(newPlayerX, transform.position.y, 0);

            anim.SetLayerWeight(4, 1);
            TurnPlayer(false, walkingDirection);
            Vector3 climbVector = new Vector3(0, yInput * climbSpeed / 2, 0);
            transform.Translate(climbVector * Time.deltaTime);
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            if (!Physics.Raycast(transform.position + Vector3.up * 2, Vector3.right * walkingDirection, 1f, lm))
            {
                HangOnLedge();
            }
        }
    }

    public virtual void HangOnLedge()
    {
        anim.SetLayerWeight(4, 1);
        anim.SetBool("IsHanging", true);
        float offset = .3f;
        float wallPosition = climbingOnObject.bounds.extents.x + offset;
        float newPlayerX = 0f;

        if (climbingOnObject.transform.position.x > transform.position.x)
        {
            newPlayerX = climbingOnObject.transform.position.x - wallPosition;
        }
        else
        {
            newPlayerX = climbingOnObject.transform.position.x + wallPosition;
        }

        transform.position = new Vector3(newPlayerX, climbingOnObject.transform.position.y + climbingOnObject.bounds.extents.y - 2.1f, 0);

        TurnPlayer(false, walkingDirection);
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

    public virtual void Falling()
    {
        if (useRootMovement)
        {
            Move(1, false);
        }
    }

    public virtual void Jump()
    {
        if (canJump && baseState == BaseState.Grounded)
        {
            rb.velocity += Vector3.up * jumpHeight + Vector3.right * xInput * 4;
            anim.SetTrigger("Jump");
        }
        if(baseState == BaseState.Climbing || baseState == BaseState.Hanging)
        {
            if(xInput * walkingDirection < 0)
            {
                print(xInput * walkingDirection);
                TurnPlayer(false, -1 * walkingDirection);
                DeActivateRootMotion();
                rb.useGravity = true;
                rb.isKinematic = false;
                anim.SetLayerWeight(4, 0);
                ResetStates();
                rb.velocity += Vector3.up * 3 + Vector3.right * xInput * 6;
            }
        }
    }

    public virtual void Die()
    {
        if (!isDead)
        {
            isDead = true;
            Destroy(gameObject);
            _GameManager.instance.PlayerDeath(true);
            OnDeathEvent();
            if (ragdoll != null)
            {
                Instantiate(ragdoll, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogError("Player ragdoll not setup!");
            }
        }
    }

    Coroutine lerpRoutine;
    public void LerpColSize(float newSize, float time)
    {
        if (time <= 0)
        {
            col.height = newSize;
        }

        else
        {
            if(lerpRoutine != null)
            {
                StopCoroutine(lerpRoutine);
            }
            lerpRoutine = StartCoroutine(LerpCZ(newSize, time));
        }
    }

    IEnumerator LerpCZ(float newSize, float time)
    {
        float elapsedTime = 0f;
        float oldSize = col.height;
        while(elapsedTime < time)
        {
            col.height = Mathf.Lerp(oldSize, newSize, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void StartClimbEvent()
    {
        ActivateRootMotion();
    }

    public virtual void StopClimbEvent()
    {
        DeActivateRootMotion();
        rb.useGravity = true;
        rb.isKinematic = false;
        anim.SetLayerWeight(4, 0);
        ResetStates();
        transform.position = new Vector3(model.transform.position.x, model.transform.position.y + 1, 0);
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
