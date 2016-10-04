using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    Rigidbody rb;
    public GameObject arrowHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(4, 3.5f, 0);
    }
	
	void Update () {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
	}

    void OnTriggerEnter(Collider col)
    {
        GameObject ah = Instantiate(arrowHit, transform.position, transform.rotation) as GameObject;
        ah.transform.parent = col.transform;
        Destroy(gameObject);
    }
}
