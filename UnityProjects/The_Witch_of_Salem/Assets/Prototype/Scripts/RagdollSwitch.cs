using UnityEngine;
using System.Collections;

public class RagdollSwitch : MonoBehaviour {

    Rigidbody[] rbs;
    bool ragdoll;

    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            if (ragdoll)
            {
                DisableRagdoll();
            }
            else
            {
                EnableRagdoll();
            }
        }
    }

    public void EnableRagdoll()
    {
        ragdoll = true;
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].isKinematic = false;
        }
        GetComponent<Animator>().enabled = false;
    }

    public void DisableRagdoll()
    {
        ragdoll = false;
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].isKinematic = true;
        }
        GetComponent<Animator>().enabled = true;
    }
}
