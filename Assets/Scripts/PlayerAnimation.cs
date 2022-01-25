using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerManager playerManagerScript;

    bool prevIsCombatMode;

    void Start()
    {
        prevIsCombatMode = playerManagerScript.GetIsCombatMode();
    }

    void FixedUpdate()
    {

        if (prevIsCombatMode != playerManagerScript.GetIsCombatMode())
        {
            if (playerManagerScript.GetIsCombatMode())
            {
                transform.RotateAround(transform.position, transform.right, -90);
            }
            else
            {
                transform.RotateAround(transform.position, transform.right, 90);
            }
        }


        prevIsCombatMode = playerManagerScript.GetIsCombatMode();
    }
}
