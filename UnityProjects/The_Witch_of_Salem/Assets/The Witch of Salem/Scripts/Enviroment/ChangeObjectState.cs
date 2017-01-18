using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeObjectState : MonoBehaviour {

    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDeactivate;

    bool used = false;

    public void Activate()
    {
        if (!used)
        {
            foreach (GameObject g in objectsToActivate)
            {
                g.SetActive(true);
            }
            foreach (GameObject g in objectsToDeactivate)
            {
                g.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            Activate();
        }
    }
}
