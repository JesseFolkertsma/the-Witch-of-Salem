using UnityEngine;
using System.Collections;

public class sdfhj : MonoBehaviour
{
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump"))
            rb.velocity = new Vector3(10, 10, 0);
            print(2345789);

    }
}