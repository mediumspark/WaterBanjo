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
        }
    }

    /// <summary>
    /// Used as a fill method for when the player is in the automatic fill range
    /// </summary>
    public void ManualFill()
    {
        OnWaterHit(null); 
    }

    /// <summary>
    /// When the hits the water interactable with water 
    /// </summary>
    /// <param name="other">
    /// The source of water
    /// </param>
    protected virtual void OnWaterHit(GameObject other)
    {

    }
}


/// <summary>
/// Wells can be filled with water if not full and if the player 
/// interacts with them when they are full they can gain a large amount of water back
/// Wells also act as respawn areas when you die if you interact with them
/// </summary>
public class Well : WaterInteractableBehavior, IInteractable
{
    public float WaterCapacity, WaterInWell;
    [SerializeField]
    private GameObject WaterLevel;

    protected override void OnWaterHit(GameObject other)
    {
        //Checks to and then adds to well capacity
        WaterInWell = WaterInWell + 0.5f < WaterCapacity ? WaterInWell + 0.5f : WaterCapacity;
        //Updates the size of the liquid in the well
        WaterLevel.transform.localScale = new Vector3(WaterLevel.transform.localScale.x, 
            ( WaterInWell / WaterCapacity ) * 0.08f % 0.09f, WaterLevel.transform.localScale.z);
    }

    //Refills the player's moisture and can be used as a respawn point
    public void OnInteract()
    {
        if(WaterInWell >= WaterCapacity)
        {
            PlayerScriptableReference.Instance.PlayerSO.MoistureLevel = PlayerScriptableReference.Instance.PlayerSO.MoistureMax; 
        }

        //Location of Well
        PlayerScriptableReference.Instance.PlayerSO.RespawnPosition = SetRespawnArea().Item2;

        //Scene of Well
        PlayerScriptableReference.Instance.PlayerSO.RespawnScene = SetRespawnArea().Item1; 

    }

    //Sets the spawn point for the player
    private Tuple<string,Vector3> SetRespawnArea()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Vector3 RespawnLocation = transform.position + HalfVector.SesquiForward;

        return Tuple.Create(currentScene, RespawnLocation);
    }
}
