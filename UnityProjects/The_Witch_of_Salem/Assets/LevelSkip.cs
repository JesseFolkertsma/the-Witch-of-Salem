using UnityEngine;
using System.Collections;

public class LevelSkip : MonoBehaviour {

	void Update () {
        if (Input.GetButtonDown("LevelSkip"))
        {
            _Player p = FindObjectOfType<_Player>();
            if(p != null)
            {
                p.transform.position = transform.position;
            }
        }
	}
}
