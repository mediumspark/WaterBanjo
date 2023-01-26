using UnityEngine;
using System.Collections.Generic;
using System.Collections; 

/// <summary>
/// Collectable to adds to the players water meter
/// </summary>
public class Moisture : MonoBehaviour, ICollectable
{
    public void OnCollect()
    {
        PlayerScriptableReference.Instance.PlayerSO.AddMoisture(PlayerScriptableReference.Instance.PlayerSO.MoistureMax * 0.1f); 
        StartCoroutine(Refresh());
    }

    private IEnumerator Refresh()
    {
        if (PlayerScriptableReference.Instance.PlayerSO.MoistureLevel < PlayerScriptableReference.Instance.PlayerSO.MoistureMax)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            yield return new WaitForSecondsRealtime(10.0f);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<Collider>().enabled = true;
        }
    }
}
