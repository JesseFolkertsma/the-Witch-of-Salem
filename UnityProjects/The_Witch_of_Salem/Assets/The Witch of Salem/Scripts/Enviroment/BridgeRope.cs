using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BridgeRope : MonoBehaviour {

	public List<Rigidbody> ropePieces = new List<Rigidbody>();
    public bool broken;

    void Start()
    {
        if (broken)
        {
            BreakRope();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Arrow>() != null)
        {
            BreakRope();
        }
    }

    public void BreakRope()
    {
        for(int i = 0; i < ropePieces.Count; i++)
        {
            ropePieces[i].isKinematic = false;
            ropePieces[i].AddExplosionForce(500, ropePieces[0].transform.position, 5);
        }
        GetComponent<Animator>().SetTrigger("HitRope");
        Destroy(GetComponent<BoxCollider>());
        broken = true;
        //GameManager.instance.lm.testInfo.bridgeDown = true;
    }
}
