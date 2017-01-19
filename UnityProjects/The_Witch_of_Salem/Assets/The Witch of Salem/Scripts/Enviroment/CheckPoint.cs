using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
    
    public bool used;

    void OnTriggerEnter(Collider col)
    {
        if (used == false)
        {
            if (col.attachedRigidbody != null)
            {
                if (col.attachedRigidbody.tag == "Player")
                {
                    _GameManager.instance.SaveGame();
                    _UIManager.instance.DisplayPopup("CheckPoint Reached!", 3f);
                    used = true;
                }
            }
        }
    }

}
