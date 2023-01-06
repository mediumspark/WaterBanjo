using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SaveLoadUtility : MonoBehaviour
{
    [SerializeField]
    private Player PlayerSlotOne, PlayerSlotTwo, PlayerSlotThree, PASaveSlot;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }


    private void SceneManager_sceneLoaded(Scene arg, LoadSceneMode arg0)
    {
        GetComponentInChildren<Animator>().Play("LevelLoaded");

        GetComponent<PlayerScriptableReference>().SavePlace();


        if (arg.buildIndex == 0)
        {
            SaveLoadUtility[] SaveLoadObjects = FindObjectsOfType<SaveLoadUtility>();
            if (SaveLoadObjects.Length > 1) Destroy(SaveLoadObjects[0].gameObject);
        }
    }

    private void ForLoadPipe(Scene arg, LoadSceneMode arg0) 
    {
        GetComponentInChildren<Animator>().Play("LevelLoaded");
        GetComponent<PlayerScriptableReference>().SavePlace();

        LevelScriptableReference _LSR = FindObjectOfType<LevelScriptableReference>();
        if(_LSR != null && _LSR.IsHubLevel)
        {
            PlayerControls.TeleportPlayer(_LSR.HubSpawnLocation);
            Vector3 Place = PlayerControls.PlayerMovement.transform.position;
            SavePlace(PlayerScriptableReference.PlayerSO, SceneManager.GetActiveScene(), Place);
        }

        if (arg.buildIndex == 0)
        {
            SaveLoadUtility[] SaveLoadObjects = FindObjectsOfType<SaveLoadUtility>();
            if (SaveLoadObjects.Length > 1) Destroy(SaveLoadObjects[0].gameObject);
        }

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }


    public void LoadFromPipe()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        SceneManager.sceneLoaded += ForLoadPipe; 
    }


    public void NewGame(Player CurrentSlot)
    {
        CurrentSlot.Reset();
        PlayerScriptableReference.PlayerSO = CurrentSlot; 
        SceneManager.LoadScene(1);
    }

    public static void LoadLevel(int level)
    {
        SceneManager.LoadScene(level); 
    }

    public static void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName); 
    }

    public Player LoadSlot(int SaveSlot)
    {
        switch (SaveSlot)
        {
            case 1:
                return PlayerSlotOne;
            case 2:
                return PlayerSlotTwo;           
            case 3:
                return PlayerSlotThree;
            case 0:
                return PASaveSlot; 
            default:
                Debug.LogError($"{SaveSlot} is not a valid save slot. Save slots are 1 - 3");
                return null; 
        }
    }

    public static void SavePlace(Player p, Scene Scene, Vector3 Position)
    {
        p.CurrentScene = Scene.name;
        p.PositioninScene = Position; 
    }
}
