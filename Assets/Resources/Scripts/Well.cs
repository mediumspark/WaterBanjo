using UnityEngine;
using System;

/// <summary>
/// Inherited by all objects that will interact with water particles (or however water is decided to be done later in the game)
/// </summary>
public class WaterInteractableBehavior : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("PlayerWater"))
        {
            OnWaterHit(other);
            Destroy(other);
        }
    }

    protected virtual void OnWaterHit(GameObject other)
    {

    }
}


/// <summary>
/// Wells can be filled with water if not full and if the player 
/// interacts with them when they are full they can gain a large amount of water back
/// </summary>
public class Well : WaterInteractableBehavior, IInteractable
{
    public float WaterCapacity, WaterInWell;
    [SerializeField]
    private GameObject WaterLevel;

    protected override void OnWaterHit(GameObject other)
    {
        WaterInWell = WaterInWell + 0.5f < WaterCapacity ? WaterInWell + 0.5f : WaterCapacity;
        WaterLevel.transform.localScale = new Vector3(WaterLevel.transform.localScale.x, 
            ( WaterInWell / WaterCapacity ) * 0.08f % 0.09f, WaterLevel.transform.localScale.z);
    }

    public void OnInteract()
    {
        if(WaterInWell >= WaterCapacity)
        {
            PlayerScriptableReference.PlayerSO.AddMoisture(WaterCapacity);
        }
    }
}
