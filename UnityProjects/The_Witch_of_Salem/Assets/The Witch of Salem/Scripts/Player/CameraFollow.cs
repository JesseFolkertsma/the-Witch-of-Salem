using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform camPos;
    public Camera cam;
    public float damping = 5;
    Vector3 character;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camPos = GameObject.Find("CameraPosition").transform;
        cam.transform.position = transform.position;
        character = camPos.parent.position;
    }

    void Update()
    {
        character = camPos.parent.position;
        camPos.transform.position = new Vector3(transform.position.x, transform.position.y + 2.5f, -10);
        cam.transform.position = Vector3.Lerp(cam.transform.position, camPos.transform.position, Time.deltaTime * damping);
    }

}
