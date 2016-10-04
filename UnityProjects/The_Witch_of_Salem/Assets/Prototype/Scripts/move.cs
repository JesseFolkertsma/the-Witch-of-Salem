using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

    public Vector3 movement;

	void Update () {
        transform.Translate(movement * Time.deltaTime);
	}
}
