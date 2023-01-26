using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    int CoinID; 

    public void OnCollect()
    {
        FindObjectOfType<LevelScriptableReference>().CoinCollected(this); 
        PlayerScriptableReference.Instance.PlayerSO.Coins++; 
        PlayerScriptableReference.Instance.PlayerSO.TotalCoins++;
        PlayerCanvas.UpdateUI(); 
        Destroy(gameObject);
    }
}
