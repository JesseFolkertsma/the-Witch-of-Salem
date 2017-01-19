using UnityEngine;
using System.Collections;

public class StairAndLever : MonoBehaviour {

    public GameObject stairs;
    public GameObject interactIndic;
    bool stairOpen;
    bool playerHasEntered;
    Vector3 idlePos;
    Vector3 openPos;
    public float speed = 6f;
    Animator anim;

    void Start()
    {
        idlePos = stairs.transform.position;
        openPos = idlePos;
        openPos.z -= 2;
        anim = GetComponent<Animator>();
    }

    public void PullLever()
    {
        stairOpen = !stairOpen;
        anim.SetTrigger("Pull");
    }

    void Update()
    {
        if (playerHasEntered)
        {
            interactIndic.SetActive(true);
            if (Input.GetButtonDown("E"))
            {
                PullLever();
            }
        }
        else
        {
            interactIndic.SetActive(false);
        }

        Vector3 pos;
        if (stairOpen)
        {
            pos = openPos;
        }
        else
        {
            pos = idlePos;
        }

        stairs.transform.position = Vector3.Lerp(stairs.transform.position, pos, Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            playerHasEntered = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            playerHasEntered = false;
        }
    }
}
