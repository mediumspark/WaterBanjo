using UnityEngine;
using System.Collections.Generic;
using System.Collections; 

/// <summary>
/// Collectable to adds to the players water meter
/// </summary>
public class Moisture : MonoBehaviour, ICollectable
{
    public int MoistureGain = 5; 

    public void OnCollect()
    {
        PlayerScriptableReference.PlayerSO.AddMoisture(MoistureGain); 
        StartCoroutine(Refresh());
    }

    private IEnumerator Refresh()
    {
        if (PlayerScriptableReference.PlayerSO.MoistureLevel < PlayerScriptableReference.PlayerSO.MoistureMax)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            yield return new WaitForSecondsRealtime(10.0f);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<Collider>().enabled = true;
        }
    }
}
