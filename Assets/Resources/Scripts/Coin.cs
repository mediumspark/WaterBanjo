using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    int CoinID; 

    public void OnCollect()
    {
        PlayerScriptableReference.PlayerSO.Coins++; 
        PlayerScriptableReference.PlayerSO.TotalCoins++;
        PlayerCanvas.UpdateUI(); 
        Destroy(gameObject);
    }
}
