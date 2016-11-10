using UnityEngine;
using System.Collections;

public class Raven : Enemy {

    public float flapStrenght;
    public float flySpeed;
    public float flapCD;
    bool canFlap = true;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	void Update()
    {
        Flap();

        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    void Flap()
    {
        if (canFlap == true)
        {
            StartCoroutine(FlapCD());

            if (CheckTargetDir(player.position) == TargetDirection.Right)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                rb.velocity = new Vector3(5, flapStrenght, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
                rb.velocity = new Vector3(-5, flapStrenght, 0);
            }
        }
    }

    IEnumerator FlapCD()
    {
        canFlap = false;
        yield return new WaitForSeconds(flapCD);
        canFlap = true;
    }
}
