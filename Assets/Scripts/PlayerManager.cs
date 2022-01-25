using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerInput playerInputScript;

    private bool isCombatMode { get; set; }

    void Start()
    {
        isCombatMode = false;
    }

    void FixedUpdate()
    {
        if (playerInputScript.GetToggleC())
        {
            isCombatMode = !isCombatMode;
        }
    }

    public bool GetIsCombatMode()
    {
        return isCombatMode;
    }
}
