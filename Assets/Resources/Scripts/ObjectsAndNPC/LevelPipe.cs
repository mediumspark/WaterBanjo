using UnityEngine;

public class LevelPipe : MonoBehaviour, IInteractable
{
    public int LevelToLoad;

    public int NumberOfCoinsToUnlock;

    public bool Unlocked;

    public GameObject PipeParticles, ClosedOffPipe;

    private void Awake()
    {
        UnlockCheck(); 
    }

    public void LoadLevel(int level)
    {
        if(PlayerScriptableReference.PlayerSO.Coins >= NumberOfCoinsToUnlock)
            SaveLoadUtility.LoadLevel(level);   
    }

    private void UnlockCheck()
    {
        Unlocked = PlayerScriptableReference.PlayerSO.Coins >= NumberOfCoinsToUnlock;

        PipeParticles.SetActive(Unlocked);
        ClosedOffPipe.SetActive(!Unlocked);
    }

    public void OnInteract()
    {
        UnlockCheck();         

        LoadLevel(LevelToLoad);
    }
}
