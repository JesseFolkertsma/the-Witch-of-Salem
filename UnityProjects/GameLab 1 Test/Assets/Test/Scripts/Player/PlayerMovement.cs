using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public enum MoveState
    {
        Walking,
        Sprinting,
        Crouching
    };

    public MoveState moveState = MoveState.Walking;

    public float crouchSpeed = 1f;
    public float walkingSpeed = 2f;
    public float sprintSpeed = 4f;

    public bool canMove = true;

    public bool isSprinting;
    public bool isCrouching;
    public bool isFalling;

    PlayerManager pm;

    bool contact;

    void Start()
    {
        pm = GetComponent<PlayerManager>();
        pm.rb = GetComponent<Rigidbody>();
        pm.anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove == true)
        {
            Movement();
        }
        pm.anim.SetFloat("Walking", Input.GetAxis("Horizontal"));
        pm.anim.SetBool("Crouch", isCrouching);
        pm.anim.SetBool("Sprint", isSprinting);

        //Jump
        if (Input.GetButtonDown("Jump") && isFalling == false)
        {
            Jump();
            pm.anim.SetTrigger("Jump");
        }

        //Check if player is touching ground
        if(Physics.Raycast(transform.position, Vector3.down, .05f))
        {
            isFalling = false;
        }
        else
        {
            isFalling = true;
        }
    }

    void Movement()
    {
        if (Input.GetButtonDown("Shift"))
        {
            moveState = MoveState.Sprinting;
        }
        else if (Input.GetButtonUp("Shift"))
        {
            moveState = MoveState.Walking;
        }
        else if (Input.GetButtonDown("Control"))
        {
            moveState = MoveState.Crouching;
        }
        else if (Input.GetButtonUp("Control"))
        {
            moveState = MoveState.Walking;
        }

        float ms = walkingSpeed;

        switch (moveState)
        {
            case MoveState.Walking:
                ms = walkingSpeed;
                isCrouching = false;
                isSprinting = false;
                break;
            case MoveState.Sprinting:
                ms = sprintSpeed;
                isSprinting = true;
                isCrouching = false;
                break;
            case MoveState.Crouching:
                ms = crouchSpeed;
                isSprinting = false;
                isCrouching = true;
                break;
        }
        if (Input.GetAxis("Horizontal") <= -.1f)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * ms);
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        if (Input.GetAxis("Horizontal") >= .1f)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * ms);
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    void Jump()
    {
        pm.rb.AddForce(Vector3.up * 300);
    }

    public void Climb()
    {
        pm.anim.SetTrigger("Climb");
        canMove = false;
        pm.rb.isKinematic = true;
        StartCoroutine(AfterClimb());
    }

    IEnumerator AfterClimb()
    {
        yield return new WaitForSeconds(.8f);
        pm.rb.isKinematic = false;
        canMove = true;
    }

    void OnCollisionEnter()
    {
        contact = true;
    }

    void OnCollisionExit()
    {
        contact = false;
    }
}
