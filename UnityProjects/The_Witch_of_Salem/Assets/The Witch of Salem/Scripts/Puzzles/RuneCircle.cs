using UnityEngine;
using System.Collections;

public class RuneCircle : MonoBehaviour {

    public enum Stance
    {
        Stance01,
        Stance02,
        Stance03
    };

    public Stance stance;
    public Vector3 stancePositions;

    public int CurrentStance
    {
        get
        {
            int s = 0;
            switch (stance)
            {
                case Stance.Stance01:
                    s = 1;
                    break;
                case Stance.Stance02:
                    s = 2;
                    break;
                case Stance.Stance03:
                    s = 3;
                    break;
            }
            return s;
        }
    }

    public void SetStance(int _stance)
    {
        if(_stance > 3)
        {
            _stance = 1;
        }
        switch (_stance)
        {
            case 1:
                stance = Stance.Stance01;
                break;
            case 2:
                stance = Stance.Stance02;
                break;
            case 3:
                stance = Stance.Stance03;
                break;
        }
    }

    void Update()
    {
        switch (stance)
        {
            case Stance.Stance01:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, stancePositions.x, 0), .1f);
                break;
            case Stance.Stance02:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, stancePositions.y, 0), .1f);
                break;
            case Stance.Stance03:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, stancePositions.z, 0), .1f);
                break;
        }
    }
}
