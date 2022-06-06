using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bitgem.VFX.StylisedWater; 

public class Well : MonoBehaviour, IInteractable
{
    public float WaterCapacity, WaterInWell;
    private GameObject WaterLevel;

    private void Awake()
    {
        WaterLevel = transform.GetChild(0).GetChild(0).gameObject;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("PlayerWater"))
        {
            WaterInWell = WaterInWell + 0.5f < WaterCapacity ? WaterInWell + 0.5f : WaterCapacity;
            WaterLevel.transform.localScale = new Vector3(WaterLevel.transform.localScale.x, WaterInWell / WaterCapacity, WaterLevel.transform.localScale.z);

            Destroy(other);
        }
    }

    public void OnInteract()
    {
        if(WaterInWell >= WaterCapacity)
        {
            PlayerScriptableReference.PlayerSO.AddMoisture(WaterCapacity);
        }
    }
}
