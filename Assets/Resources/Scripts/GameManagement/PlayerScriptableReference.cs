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
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
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

    public static void PlayLevelLoad()
    {
        FindObjectOfType<PlayerScriptableReference>().GetComponentInChildren<Animator>().Play("LevelLoad"); 
    }

    private void SceneManager_sceneLoaded(Scene arg, LoadSceneMode arg0)
    {
        GetComponentInChildren<Animator>().Play("LevelLoaded");

        SavePlace();
        if (arg.buildIndex == 0)
        {
            SaveLoadUtility[] SaveLoadObjects = FindObjectsOfType<SaveLoadUtility>();
            if(SaveLoadObjects.Length > 1) Destroy(SaveLoadObjects[0].gameObject);
        }
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
