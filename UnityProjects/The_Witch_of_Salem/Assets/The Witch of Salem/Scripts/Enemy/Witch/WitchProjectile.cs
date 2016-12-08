using UnityEngine;
using System.Collections;

public class WitchProjectile : MonoBehaviour {

    Transform target;
    public float speed;
    public float turnSpeed;
    Rigidbody rb;

    Vector3 t;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
    }

    void Update()
    {
        if (target != null)
        {
            FollowTarget();
        }
        else
        {
            Debug.LogWarning(gameObject.name + "'s target not found");
        }
    }

    void FollowTarget()
    {
        t = Vector3.Lerp(t, target.position + Vector3.up * 1.5f, Time.deltaTime * turnSpeed);

        transform.LookAt(t);
        //transform.Translate(transform.forward * Time.deltaTime * speed);
        //rb.AddForce(transform.forward * Time.deltaTime * speed);
        rb.velocity = transform.forward * speed;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.attachedRigidbody)
        {
            if (col.attachedRigidbody.GetComponent<_Player>())
            {
                col.attachedRigidbody.GetComponent<_Player>().TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }
}
