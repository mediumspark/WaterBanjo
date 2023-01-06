using UnityEngine;
using UnityEngine.SceneManagement; 

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
        if (PlayerScriptableReference.PlayerSO.Coins >= NumberOfCoinsToUnlock)
        {
            FindObjectOfType<SaveLoadUtility>().LoadFromPipe(); 
            SaveLoadUtility.LoadLevel(level);
        }
    }

    private void UnlockCheck()
    {
        Unlocked = PlayerScriptableReference.PlayerSO.Coins >= NumberOfCoinsToUnlock;

        PipeParticles.SetActive(Unlocked);
        ClosedOffPipe.SetActive(!Unlocked);
    }

    public void OnInteract()
    {
        LevelScriptableReference _LSR = FindObjectOfType<LevelScriptableReference>();

        if (_LSR != null && _LSR.IsHubLevel)
            _LSR.SetSpawnInLocation(transform.position + Vector3.forward); 

        UnlockCheck();
        LoadLevel(LevelToLoad);
    }
}
