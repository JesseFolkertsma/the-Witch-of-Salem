using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    public int checkPointI;
    public bool used;

    void OnTriggerEnter(Collider col)
    {
        if (used == false)
        {
            if (col.attachedRigidbody != null)
            {
                if (col.attachedRigidbody.tag == "Player")
                {
                    print("SAVEGAME");
                    used = true;
                    GameManager.instance.currentCheckpoint = checkPointI;
                    GameManager.instance.slSystem.SaveGame(GameManager.instance.playerName);
                    GameManager.instance.popup.DisplayPopup("CheckPoint Reached", 3f);
                }
            }
        }
    }

}
