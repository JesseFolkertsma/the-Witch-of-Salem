using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

    public enum PuzzleType
    {
        Symbol,
        LightStone
    };

    public PuzzleType puzzleType = PuzzleType.LightStone;

    Animator anim;
    Collider enter;

    public bool isUp = true;
    public LightStone attachedLightStone;
    public RuneCircle attackedRuneCircle;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Effect()
    {
        switch (puzzleType)
        {
            case PuzzleType.LightStone:
                if (attachedLightStone != null)
                {
                    attachedLightStone.Activate(true);
                }
                break;
            case PuzzleType.Symbol:
                if (attackedRuneCircle != null)
                {
                    attackedRuneCircle.SetStance(attackedRuneCircle.CurrentStance + 1);
                }
                break;
        }
    }

    void PlateDown()
    {
        isUp = false;
        anim.SetBool("IsUp", isUp);
        Effect();
    }

    void PlateUp()
    {
        isUp = true;
        anim.SetBool("IsUp", isUp);
    }

    void OnTriggerEnter(Collider col)
    {
        if (isUp)
        {
            if (col.attachedRigidbody)
            {
                if (col.attachedRigidbody.GetComponent<_Player>())
                {
                    PlateDown();
                    enter = col;
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(enter != null)
        {
            if(enter == col)
            {
                PlateUp();
            }
        }
    }
}
