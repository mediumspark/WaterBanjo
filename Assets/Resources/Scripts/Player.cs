using System.Collections.Generic;
using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : ScriptableObject
{
    [SerializeField]
    private float _moistureLevel;
    public float MoistureLevel
    {
        get { return _moistureLevel; }
        set
        {
            _moistureLevel = value;
            Hydro.HydroLevel.transform.localScale = new Vector3(Hydro.HydroLevel.transform.localScale.x,
                (_moistureLevel / MoistureMax) * 1.2f % 1.21f, Hydro.HydroLevel.transform.localScale.z);
        }
    }
    public float MoistureMax;

    [SerializeField]
    private string InScene;
    public string CurrentScene { get => InScene; set => InScene = value; }

    [SerializeField]
    private Vector3 LocationInScene = Vector3.zero;
    public Vector3 PositioninScene { get => LocationInScene; set => LocationInScene = value; }


    public List<Nozzle> Nozzles;
    public int Coins, TotalCoins;

    public string RespawnScene;
    public Vector3 RespawnPosition;

    private bool Dead = false; 


    public IEnumerator OnDeath()
    {
        if (!Dead)
        {
            Dead = true; 
            //Fade to black Transition
            yield return new WaitForSeconds(0.5f);

            //Load respawn scene
            SceneManager.LoadScene(RespawnScene);

            yield return new WaitForSeconds(1.5f);
            //Move Player to Spawn
            PlayerControls.TeleportPlayer(RespawnPosition);
            //Reset Player Stats
            Refresh();

            //Fade from Black
            yield return new WaitForSeconds(0.5f);
            Dead = false; 
        }
    }


    public void Refresh()
    {
        MoistureLevel = MoistureMax; 
        //Using this until I can decide whether or not I'll add a more robust health system
    }

    public void AddMoisture(float AddedWater)
    {
        if(_moistureLevel + AddedWater <= MoistureMax)
        {
            MoistureLevel += AddedWater; 
        }
        else
        {
            MoistureLevel = MoistureMax;
        }
        PlayerCanvas.UpdateUI();
    }

    public void Reset()
    {
        InScene = SceneManager.GetSceneAt(1).name;

        MoistureMax = 50.0f;
        MoistureLevel = MoistureMax;
        Coins = 0;
        TotalCoins = 0; 

        foreach(Nozzle nozzle in Nozzles)
        {
            nozzle.Unlocked = false; 
        }

        Nozzles[0].Unlocked = true; 
        Nozzles[1].Unlocked = true; 
    }
}
