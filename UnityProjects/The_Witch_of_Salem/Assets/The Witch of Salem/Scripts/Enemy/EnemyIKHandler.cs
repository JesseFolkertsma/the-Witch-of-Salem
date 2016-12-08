using UnityEngine;
using System.Collections;

public class EnemyIKHandler : MonoBehaviour {

    Animator anim;

    public float headWeight;
    public float bodyWeight;

    public Transform target;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public enum EnemyType
    {
        Farmer,
        Golem,
        Witch
    };

    public EnemyType enemyType = EnemyType.Farmer;

    void OnAnimatorIK()
    {
        switch (enemyType)
        {
            case EnemyType.Farmer:
                break;
            case EnemyType.Golem:
                break;
            case EnemyType.Witch:
                anim.SetLookAtWeight(1, bodyWeight, headWeight, 1, .5f);
                anim.SetLookAtPosition(target.position);
                break;
        }
    }
}
