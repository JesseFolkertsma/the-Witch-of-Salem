using UnityEngine;
using System.Collections;

public class LookatPoint : MonoBehaviour {

    public Camera cam;
    public Vector3 pos;

	void Update () {
        pos.x = Input.mousePosition.x;
        pos.y = Input.mousePosition.y;
        pos.z = Mathf.Abs(cam.transform.position.z);
        transform.position = cam.ScreenToWorldPoint(pos);
    }
}
