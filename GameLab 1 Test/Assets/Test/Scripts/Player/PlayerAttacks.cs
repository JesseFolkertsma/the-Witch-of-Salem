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

    public Transform backBone;

    public bool useBow = false;

    void Start()
    {
        pm = GetComponent<PlayerManager>();
    }

	void Update()
    {
        pm.anim.SetBool("UseBow", useBow);
    }

    void LateUpdate()
    {

        if (weaponType == WeaponType.Melee)
        {
            Melee();
            text.text = "Melee";

            if (Input.GetButtonDown("Fire3"))
            {
                weaponType = WeaponType.Ranged;
                pm.anim.SetTrigger("DrawArrow");
                return;
            }
        }

        if (weaponType == WeaponType.Ranged)
        {
            Ranged();
            text.text = "Ranged";

            if (Input.GetButtonDown("Fire3"))
            {
                weaponType = WeaponType.Melee;
                return;
            }
        }
    }

    void Melee()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            print("Attack");
            if (pm.pm.isSprinting == true)
            {
                SprintAttack();
            }
            if (pm.pm.isSprinting == false)
            {
                Attack();
            }
        }
    }

    void Ranged()
    {
        if (Input.GetButton("Fire2"))
        {
            backBone.LookAt(pm.lp.transform.position);
            useBow = true;
        }
        else
        {
            useBow = false;
        }
    }

    void Attack()
    {
        pm.anim.SetTrigger("Attack");
        pm.pm.canMove = false;
        StartCoroutine(AfterAttack());
    }

    IEnumerator AfterAttack()
    {
        yield return new WaitForSeconds(1.2f);
        pm.pm.canMove = true;
    }

    void SprintAttack()
    {
        pm.anim.SetTrigger("Attack");
    }
}
