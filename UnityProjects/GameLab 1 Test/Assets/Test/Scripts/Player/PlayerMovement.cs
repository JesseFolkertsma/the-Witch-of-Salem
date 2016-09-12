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
    public float sprintSpeed = 5f;

    public Vector3 movement;

    public bool canMove = true;
    public bool isFalling;

    public float state;

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

        pm.anim.SetFloat("Walking", movement.x);
        pm.anim.SetFloat("Speed", state);
        

        //Check if player is touching ground
        if(Physics.Raycast(pm.playerMiddle.position, Vector3.down, 1.1f))
        {
            isFalling = false;
        }
        else
        {
            isFalling = true;
        }

        Climb();
        CheckInput();
    }

    void CheckInput()
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

        //Jump
        if (Input.GetButtonDown("Jump") && isFalling == false)
        {
            Jump();
            pm.anim.SetTrigger("Jump");
        }

        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    }

    void Movement()
    {
        float ms = CheckMovementSpeed();

        if (pm.la.look == true)
        {
            //Walk right and look at point
            if (pm.la.IsRight() == true)
            {
                //Backward
                if (movement.x <= -.1f)
                {
                    transform.Translate(Vector3.back * Time.deltaTime * ms);
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                //Forward
                if (movement.x >= .1f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * ms);
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
            }
            //Walk left and look at point
            if (pm.la.IsRight() == false)
            {
                //Backward
                if (movement.x >= .1f)
                {
                    transform.Translate(Vector3.back * Time.deltaTime * ms);
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                //Forward
                if (movement.x <= -.1f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * ms);
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                }
            }
        }
        else
        {
            // Walk left
            if (movement.x >= .1f)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * ms);
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            // Walk Right
            if (movement.x <= -.1f)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * ms);
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }

    void Jump()
    {
        pm.rb.isKinematic = false;
        pm.rb.AddForce(Vector3.up * 300);
    }

    void Climb()
    {
        Debug.DrawRay(pm.playerMiddle.position, transform.forward);
        Debug.DrawRay(pm.playerMiddle.position + new Vector3(0, 1, 0), transform.forward);
        Debug.DrawRay(pm.playerMiddle.position, transform.forward + -transform.up);

        if (Physics.Raycast(pm.playerMiddle.position, transform.forward + -transform.up, .5f) && Physics.Raycast(pm.playerMiddle.position, transform.forward, .5f) && !Physics.Raycast(pm.playerMiddle.position + new Vector3(0,1,0), transform.forward, 3f))
        {
            pm.rb.isKinematic = true;
            canMove = false;
            isFalling = false;
            movement.x = 0;
            transform.Translate(movement * Time.deltaTime * 2);
            print("GET DA CAMERA MOM IM CLIMBING");
        }
        else
        {
            pm.rb.isKinematic = false;
            canMove = true;
        }
    }

    float CheckMovementSpeed()
    {
        float ms = 0f;
        switch (moveState)
        {
            case MoveState.Walking:
                ms = walkingSpeed;
                state = Mathf.Lerp(state, -0.01f, .05f);
                break;
            case MoveState.Sprinting:
                ms = sprintSpeed;
                state = Mathf.Lerp(state, 1f, .05f);
                break;
            case MoveState.Crouching:
                ms = crouchSpeed;
                state = Mathf.Lerp(state, -1f, .05f);
                break;
        }
        return ms;
    }
}
