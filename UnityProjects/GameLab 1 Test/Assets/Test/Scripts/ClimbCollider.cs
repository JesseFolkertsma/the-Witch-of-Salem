using UnityEngine;
using System.Collections;

public class ClimbCollider : MonoBehaviour {

    public enum Direction
    {
        Left,
        Right
    };

    public Direction direction;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            col.GetComponent<PlayerMovement>().Climb();
            col.transform.position = transform.position;
            print("CLIMB");
        }
    }
}
