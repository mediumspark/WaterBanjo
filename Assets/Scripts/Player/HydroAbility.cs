using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroAbility : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem ParticleSystemPrefab;
    protected ParticleSystem PS;


    protected virtual void OnEnable()
    {
        PS = Instantiate(ParticleSystemPrefab, transform.parent); 
    }

    protected virtual void OnDisable()
    {
        Destroy(PS); 
    }
}
