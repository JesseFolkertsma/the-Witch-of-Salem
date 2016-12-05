using UnityEngine;
using System.Collections;

public class Apple : MonoBehaviour {

    bool used;

	void OnCollisionEnter(Collision col)
    {
        if (col.collider.attachedRigidbody && !used)
        {
            if(col.collider.attachedRigidbody.tag == "Player")
            {
                used = true;
                col.collider.attachedRigidbody.GetComponent<_Player>().apples++;
                _UIManager.instance.UpdateUI();
                _UIManager.instance.DisplayPopup("Picked up apple", 2f);
                Destroy(gameObject);
            }
        }
    }
}
