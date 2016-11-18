using UnityEngine;
using System.Collections;

public class _PlayerMouse : MonoBehaviour {

    void Update()
    {
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z));
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }

	public Vector3 GetPosition
    {
        get
        {
            return transform.position;
        }
    }
}
