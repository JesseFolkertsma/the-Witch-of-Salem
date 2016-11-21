using UnityEngine;
using System.Collections;

public class Apple : MonoBehaviour {

	void OnCollisionEnter(Collision col)
    {
        if (col.collider.attachedRigidbody)
        {
            if(col.collider.attachedRigidbody.tag == "Player")
            {
                col.collider.attachedRigidbody.GetComponent<PlayerStateMachine>().ps.apples++;
                GameManager.instance.popup.DisplayPopup("Picked up apple", 2f);
                Destroy(gameObject);
            }
        }
    }
}
