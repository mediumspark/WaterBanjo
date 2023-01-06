using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroDash : HydroAbility
{
    PlayerMovement PM;
    protected void Awake()
    {
        PM = FindObjectOfType<PlayerMovement>();
    }

    protected override void OnDisable()
    {
        PM.isDashing = false; 
    }

    protected override void OnEnable()
    {
        PM.isDashing = true;
    }
}
