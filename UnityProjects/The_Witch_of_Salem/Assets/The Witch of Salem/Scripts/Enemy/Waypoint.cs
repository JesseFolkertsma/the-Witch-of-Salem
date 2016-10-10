using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
    
    void OnTriggerEnter(Collider col)
    {
        print(col.name);
        if (col.GetComponent<Golem>() != null)
        {
            GroundEnemy ge = col.GetComponent<GroundEnemy>();
            ge.currentwp++;
            if (ge.currentwp > ge.waypoints.Count - 1)
            {
                ge.currentwp = 0;
            }
        }
    }
}
