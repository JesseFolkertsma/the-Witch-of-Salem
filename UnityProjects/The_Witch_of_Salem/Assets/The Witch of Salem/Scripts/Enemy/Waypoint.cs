using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
    
    void OnTriggerEnter(Collider col)
    {
        if (col.attachedRigidbody.GetComponent<GroundEnemy>() != null)
        {
            GroundEnemy ge = col.attachedRigidbody.GetComponent<GroundEnemy>();
            ge.currentwp++;
            if (ge.currentwp > ge.waypoints.Count - 1)
            {
                ge.currentwp = 0;
            }
        }
    }
}
