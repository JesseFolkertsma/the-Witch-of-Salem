using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

    public float downPos;
    public float upPos;

    public bool isUp;
    float timer;
    public float cd = 2f;
    Vector3 v; 

    void Start()
    {
        v = transform.parent.position;
        upPos += v.y;
        downPos += v.y;
    }

    void Update()
    {
        if (isUp)
        {
            v.y = upPos;
        }
        else
        {
            v.y = downPos;
        }
        
        transform.position = Vector3.Lerp(transform.position, v, Time.deltaTime * 5f);

        if(timer < Time.time)
        {
            timer = Time.time + cd;
            MoveSpike();
        }
    }

    void MoveSpike()
    {
        isUp = !isUp;
    }

    float hitCD;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && hitCD < Time.time)
        {
            col.GetComponent<_Player>().TakeDamage(1);
            hitCD = Time.time + 1f;
        }
    }
}
