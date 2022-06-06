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
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void LoadPlayer(int SaveSlot)
    {
        Player = GetComponent<SaveLoadUtility>().LoadSlot(SaveSlot);
        PlayerSO = Player;
        SceneManager.LoadScene(Player.CurrentScene);//Player.CurrentScene.name);
    }

    private void SceneManager_sceneLoaded(Scene arg, LoadSceneMode arg0)
    {
        SavePlace(); 
    }

    public void SavePlace()
    {
        if (FindObjectOfType<PlayerMovement>())
        {

            Vector3 Place = FindObjectOfType<PlayerMovement>().transform.position = PlayerSO.PositioninScene; 
            SaveLoadUtility.SavePlace(PlayerSO, SceneManager.GetActiveScene(), Place);
        }
    }
}
