using UnityEngine;
using System.Collections;

public class HouseEnter : MonoBehaviour {

    public Transform house;
    public GameObject canEnterObject;

    Transform player;

    bool hasPlayer;
    bool canEnter;

    void Start()
    {
        canEnterObject.SetActive(false);
    }

	void EnterDoor()
    {
        player.position = house.position;
    }

    void Update()
    {
        if (hasPlayer)
        {
            if (Input.GetButtonDown("E") && canEnter)
            {
                EnterDoor();
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.attachedRigidbody != null && !hasPlayer)
        {
            if (col.attachedRigidbody.tag == "Player")
            {
                player = col.attachedRigidbody.transform;
                hasPlayer = true;
                canEnter = true;
                canEnterObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.attachedRigidbody.transform == player)
        {
            canEnter = false;
            hasPlayer = false;
            canEnterObject.SetActive(false);
        }
    }
}
