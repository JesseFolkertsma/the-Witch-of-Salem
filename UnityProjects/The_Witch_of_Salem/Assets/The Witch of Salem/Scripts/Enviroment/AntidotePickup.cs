using UnityEngine;
using System.Collections;

public class AntidotePickup : MonoBehaviour {
    public GameObject interactOrb;
    public GameObject deactivate;
    public GameObject activate;
    bool playerHasEntered;

    public void Activate()
    {
        deactivate.SetActive(false);
        activate.SetActive(true);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerHasEntered)
        {
            interactOrb.SetActive(true);
            if (Input.GetButtonDown("E"))
            {
                Activate();
            }
        }
        else
        {
            interactOrb.SetActive(false);
        }
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
