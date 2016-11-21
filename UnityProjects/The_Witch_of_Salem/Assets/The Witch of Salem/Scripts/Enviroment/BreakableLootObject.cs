using UnityEngine;
using System.Collections;

public class BreakableLootObject : MonoBehaviour {

    public GameObject pieces;
    public float destroyForce = 500;
    public GameObject apple;

	public void Break(Vector3 pos)
    {
        pieces = Instantiate(pieces, transform.position, Quaternion.identity) as GameObject;
        pieces.transform.parent = null;
        Rigidbody[] rbs = pieces.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].AddExplosionForce(destroyForce, pos, 5);
        }
        GameObject g = Instantiate(apple, null) as GameObject;
        g.transform.position = new Vector3(transform.position.x, transform.position.y,0);
        GetComponent<AudioSource>().Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        Destroy(pieces, 10);
        Destroy(gameObject, 5);
    }
}
