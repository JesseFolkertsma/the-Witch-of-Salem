using UnityEngine;
using System.Collections;

public class HouseEnter : MonoBehaviour {

    public Transform house;

    Transform player;

    bool hasPlayer;
    bool canEnter;

	void EnterDoor()
    {
        player.position = house.position;
        print("Enter");
    }

    void Update()
    {
        if (Input.GetButtonDown("E") && canEnter)
        {
            EnterDoor();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        print(col.name);
        if (col.attachedRigidbody != null)
        {
            if (col.attachedRigidbody.tag == "Player")
            {
                if (!hasPlayer)
                {
                    player = col.attachedRigidbody.transform;
                    hasPlayer = true;
                    print("eeeee");
                }
                GameManager.instance.popup.DisplayPopup("Press 'E' to enter door", 2);
                canEnter = true;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        print(col.name);
        if (col.attachedRigidbody.transform == player)
        {
            canEnter = false;
        }
    }
}
