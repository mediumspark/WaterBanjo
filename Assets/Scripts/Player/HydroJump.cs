using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroJump : HydroAbility
{
    PlayerMovement PM;
    protected void Awake()
    {
        PM = FindObjectOfType<PlayerMovement>();
    }

    protected override void OnDisable()
    {
        PM.Launch = false; 
    }

    protected override void OnEnable()
    {
        PM.Launch = true;
    }
}
