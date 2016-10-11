using UnityEngine;
using System.Collections;

public class RagdollSwitch : MonoBehaviour {

    public Rigidbody[] rbs;
    public Collider[] cols;
    public Rigidbody mainRB;
    public Collider mainCol;
    public Animator anim;
    public bool ragdoll;

    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        cols = GetComponentsInChildren<Collider>();
        mainRB = GetComponent<Rigidbody>();
        mainCol = GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();
        DisableRagdoll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
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
            cols[i].enabled = true;
        }
        anim.enabled = false;
        mainRB.isKinematic = true;
        mainCol.enabled = false;
    }

    public void PlayFall()
    {
        StopAllCoroutines();
        StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        EnableRagdoll();
        yield return new WaitForSeconds(5f);
        DisableRagdoll();
    }

    public void DisableRagdoll()
    {
        ragdoll = false;
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].isKinematic = true;
            cols[i].enabled = false;
        }
        anim.enabled = true;
        mainRB.isKinematic = false;
        mainCol.enabled = true;
    }
}
