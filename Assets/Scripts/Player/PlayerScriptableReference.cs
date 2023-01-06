using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScriptableReference : MonoBehaviour
{

    private Player Player;
    public static Player PlayerSO;

    private void Awake()
    {
        PlayerSO = Player;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadPlayer(int SaveSlot)
    {
        GetComponentInChildren<Animator>().Play("LevelLoad");

        Player = GetComponent<SaveLoadUtility>().LoadSlot(SaveSlot);
        PlayerSO = Player;
        SceneManager.LoadScene(Player.CurrentScene);//Player.CurrentScene.name);
    }

    public void LoadPracticeArena(int SaveSlot)
    {
        GetComponentInChildren<Animator>().Play("LevelLoad");
        Player = GetComponent<SaveLoadUtility>().LoadSlot(SaveSlot);
        PlayerSO = Player;
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
                Debug.LogWarning("Error thrown attempting to save positino of Player");
            }
        }
    }
}
