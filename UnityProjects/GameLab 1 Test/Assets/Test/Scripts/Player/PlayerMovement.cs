using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public enum MoveState
    {
        Walking,
        Sprinting,
        Crouching,
        Climbing
    };

    public MoveState moveState = MoveState.Walking;

    public float crouchSpeed = 1f;
    public float walkingSpeed = 2f;
    public float sprintSpeed = 5f;

    public Vector3 movement;

    public bool canMove = true;
    public bool canClimb = true;
    public bool isFalling = false;
    public bool isClimbing = false;

    float jumpHeigt = 5;
    float jumpXForce = 0;

    public float state;

    PlayerManager pm;

    bool contact;

    void Start()
    {
        pm = GetComponent<PlayerManager>();
    }

    void Update()
    {
        Movement(canMove);

        //Check if player is touching ground
        if(Physics.Raycast(pm.playerMiddle.position, Vector3.down, 1.1f))
        {
            isFalling = false;
        }
        else
        {
            isFalling = true;
        }

        Climb(canClimb);
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

    void Movement(bool b)
    {
        if (b)
        {
            pm.anim.SetFloat("Walking", movement.x);
            pm.anim.SetFloat("Speed", state);

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
    }

    void Jump()
    {
        print("Jumping!");

        pm.rb.isKinematic = false;
        pm.rb.velocity = new Vector3(jumpXForce, jumpHeigt, 0);
        StartCoroutine(AfterJump());
    }

    IEnumerator AfterJump()
    {
        canClimb = false;
        yield return new WaitForSeconds(.2f);
        canClimb = true;
    }

    void Climb(bool b)
    {
        Debug.DrawRay(pm.playerMiddle.position + new Vector3(0, .5f, 0), transform.forward);
        Debug.DrawRay(pm.playerMiddle.position + new Vector3(0, 1.5f, 0), transform.forward);

        pm.anim.SetFloat("ClimbSpeed", Input.GetAxis("Vertical"));
        pm.anim.SetBool("IsClimbing", isClimbing);

        if (b)
        {
            RaycastHit ray;

            if (Physics.Raycast(pm.playerMiddle.position + new Vector3(0, 0f, 0), transform.forward, out ray, .5f) && !Physics.Raycast(pm.playerMiddle.position + new Vector3(0, 1.5f, 0), transform.forward, 3f) && canClimb == true)
            {
                isClimbing = true;
                pm.rb.isKinematic = true;
                canMove = false;
                isFalling = false;

                movement.x = 0;
                transform.Translate(movement * Time.deltaTime * 2);

                Vector3 pospos;

                jumpHeigt = 7;

                //Check is Player is right or left from object
                if (ray.transform.position.x - transform.position.x < 0) {
                    jumpXForce = .5f;
                    pospos = ray.transform.position + new Vector3(ray.collider.bounds.extents.x, ray.collider.bounds.extents.y, 0);
                }
                else {
                    jumpXForce = -.5f;
                    pospos = ray.transform.position + new Vector3(-ray.collider.bounds.extents.x, ray.collider.bounds.extents.y, 0);
                }

                if(!Physics.Raycast(pm.playerMiddle.position + new Vector3(0, .1f, 0), transform.forward, .5f))
                {
                    StartCoroutine(ClimbUp());
                    print("CLIMB BITCH CLIMB");
                }

                Debug.DrawRay(pospos, transform.up, Color.red);
                
                PlayerIK.instance.useIK = true;
                PlayerIK.instance.hPos = pospos;
            }
            else
            {
                PlayerIK.instance.useIK = false;
                pm.rb.isKinematic = false;
                canMove = true;
                isClimbing = false;
                jumpHeigt = 5;
                jumpXForce = 0;
            }
        }
    }

    IEnumerator ClimbUp()
    {
        canClimb = false;
        pm.anim.applyRootMotion = true;
        pm.anim.SetTrigger("Climb");
        GetComponentInChildren<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        pm.anim.applyRootMotion = false;
        canClimb = true;
        GetComponentInChildren<BoxCollider>().enabled = true;
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
