using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SaveLoadUtility : MonoBehaviour
{
    [SerializeField]
    private Player PlayerSlotOne, PlayerSlotTwo, PlayerSlotThree;
    
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
