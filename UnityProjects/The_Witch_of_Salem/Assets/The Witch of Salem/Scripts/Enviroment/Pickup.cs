using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    PlayerStateMachine player;
	
    void OnCollisionEnter (Collision col)
    {
        if(col.collider.attachedRigidbody != null)
        {
            if (col.collider.attachedRigidbody.GetComponent<PlayerStateMachine>() != null)
            {
                player = col.collider.attachedRigidbody.GetComponent<PlayerStateMachine>();
                GameManager.instance.popup.DisplayPopup("Picked up block", 2);
                Destroy(gameObject);
            }
        }
    }
}
