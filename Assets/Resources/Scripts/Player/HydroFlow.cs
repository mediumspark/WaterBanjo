using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simply removes the parent so the water continues without being tied to the parent
/// </summary>
public class HydroFlow : MonoBehaviour
{
    void Start()
    {
        transform.parent = null; 
    }
}
