using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScriptableReference : MonoBehaviour
{
    public static PlayerScriptableReference Instance; 

    private Player Player;
    public Player PlayerSO { get => Player; set => Player = value;  }

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void LoadPlayer(int SaveSlot)
    {
        GetComponentInChildren<Animator>().Play("LevelLoad");
        PlayerSO = GetComponent<SaveLoadUtility>().LoadSlot(SaveSlot);
        SceneManager.LoadScene(Player.CurrentScene);//Player.CurrentScene.name);
    }

    public void LoadPracticeArena(int SaveSlot)
    {
        GetComponentInChildren<Animator>().Play("LevelLoad");
        PlayerSO = GetComponent<SaveLoadUtility>().LoadSlot(SaveSlot);
        SceneManager.LoadScene("PracticeArea");
    }

    public void LoadHubLevel(HubLevel hub)
    {
        PlayerControls.TeleportPlayer(hub.SpawnInLocation);
    }

    public static void PlayLevelLoad()
    {
        FindObjectOfType<PlayerScriptableReference>().GetComponentInChildren<Animator>().Play("LevelLoad"); 
    }

    public void SavePlace()
    {
        if (PlayerControls.PlayerMovement)
        {
            try
            {
                PlayerControls.PlayerMovement.transform.position = !Player.Dead ? PlayerSO.PositioninScene : PlayerSO.RespawnPosition;
                Vector3 Place = PlayerControls.PlayerMovement.transform.position;
                SaveLoadUtility.SavePlace(PlayerSO, SceneManager.GetActiveScene(), Place);
            }
            catch
            {
                Debug.LogWarning("Error thrown attempting to save position of Player");
            }
        }
    }
}
