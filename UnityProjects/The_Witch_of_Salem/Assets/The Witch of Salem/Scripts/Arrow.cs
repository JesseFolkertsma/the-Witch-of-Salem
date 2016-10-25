using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    public Rigidbody rb;
    public GameObject arrowHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        transform.SetParent(null);
    }
	
	void Update () {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
	}

    public void Shoot(Vector3 direction, float strenght)
    {
        rb.velocity += direction * strenght;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.attachedRigidbody != null)
        {
            if (col.attachedRigidbody.GetComponent<Enemy>() != null)
            {
                col.attachedRigidbody.GetComponent<Enemy>().health -= 30;
            }
        }

        GameObject go = Instantiate(new GameObject(), transform.position - rb.velocity.normalized / 10, Quaternion.identity) as GameObject;
        go.name = "ArrowObject";
        go.transform.parent = col.transform;
        GameObject ah = Instantiate(arrowHit, go.transform.position, transform.rotation) as GameObject;
        ah.transform.parent = go.transform;
        Destroy(go, 20f);
        Destroy(gameObject);
    }
}
