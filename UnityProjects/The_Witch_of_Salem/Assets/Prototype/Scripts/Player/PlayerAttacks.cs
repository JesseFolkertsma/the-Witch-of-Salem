using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAttacks : MonoBehaviour {

    public enum WeaponType
    {
        Melee,
        Ranged
    };

    public WeaponType weaponType;

    public Text text;

    PlayerManager pm;

    public bool useBow = false;

    public int combo;

    void Start()
    {
        pm = GetComponent<PlayerManager>();
        //text = GameObject.Find("Text").GetComponent<Text>();
    }

	void Update()
    {
        pm.anim.SetBool("UseBow", useBow);
        
        if (weaponType == WeaponType.Melee)
        {
            Melee();
            //text.text = "Melee";

            if (Input.GetButtonDown("Fire3"))
            {
                weaponType = WeaponType.Ranged;
                return;
            }
        }

        if (weaponType == WeaponType.Ranged)
        {
            //text.text = "Ranged";

            if (Input.GetButtonDown("Fire3"))
            {
                weaponType = WeaponType.Melee;
                return;
            }
        }
    }

    void LateUpdate()
    {
        if (weaponType == WeaponType.Ranged)
        {
            Ranged();
            //text.text = "Ranged";
        }
    }

    void Melee()
    {
        if (Input.GetButtonDown("Fire1") && pm.pMove.CheckMoveState() == 0)
        {
            print("Attack");
            Attack();
        }
        if (Input.GetButtonDown("Fire1") && pm.pMove.CheckMoveState() == 1)
        {
            print("Sprint Attack");
            SprintAttack();
        }
    }

    void Ranged()
    {
        if (Input.GetButton("Fire2") && pm.pMove.CheckMoveState() == 1)
        {
            pm.la.look = true;
            useBow = true;
        }
        else
        {
            pm.la.look = false;
            useBow = false;
        }
    }

    void Attack()
    {
        StartCoroutine(AfterAttack());
    }

    IEnumerator AfterAttack()
    {

        pm.anim.SetTrigger("Attack");
        pm.pMove.canMove = false;
        yield return new WaitForSeconds(.6f);
        pm.pMove.canMove = true;
    }

    void SprintAttack()
    {
        pm.anim.SetTrigger("Attack");
    }
}
