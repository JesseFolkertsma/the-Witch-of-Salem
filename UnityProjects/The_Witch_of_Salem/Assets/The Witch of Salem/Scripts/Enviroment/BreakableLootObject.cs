using UnityEngine;
using System.Collections;

public class BreakableLootObject : MonoBehaviour {

    public GameObject pieces;
    public float destroyForce = 500;

	public void Break(Vector3 pos)
    {
        pieces = Instantiate(pieces, transform.position, Quaternion.identity) as GameObject;
        pieces.transform.parent = null;
        Rigidbody[] rbs = pieces.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].AddExplosionForce(destroyForce, pos, 5);
        }
        Destroy(pieces, 10);
        Destroy(gameObject);
    }
}
