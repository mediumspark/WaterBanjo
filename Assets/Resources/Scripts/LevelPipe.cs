using UnityEngine;

public class LevelPipe : MonoBehaviour, IInteractable
{
    public int LevelToLoad;

    public int NumberOfCoinsToUnlock; 

    public void LoadLevel(int level)
    {
        if(PlayerScriptableReference.PlayerSO.Coins >= NumberOfCoinsToUnlock)
            SaveLoadUtility.LoadLevel(level);   
    }

    public void OnInteract()
    {
        LoadLevel(LevelToLoad);
    }
}
