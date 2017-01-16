using UnityEngine;
using System.Collections;

public class ArrowPickup : MonoBehaviour {

    public int amount;

	void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Player")
        {
            _Player player = col.transform.GetComponent<_Player>();
            player.arrows += amount;
            _UIManager.instance.UpdateUI();
            Destroy(gameObject);
        }
    }
}
