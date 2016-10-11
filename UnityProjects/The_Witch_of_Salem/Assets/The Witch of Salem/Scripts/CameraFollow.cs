using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform camPos;
    public Camera cam;
    public float damping = .1f;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camPos = GameObject.Find("CameraPosition").transform;
        cam.transform.position = transform.position;
    }

    void Update()
    {
        camPos.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, -12);
        cam.transform.position = Vector3.Lerp(cam.transform.position, camPos.transform.position, damping);
    }

}
