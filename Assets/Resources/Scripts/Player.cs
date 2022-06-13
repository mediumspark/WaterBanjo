using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : ScriptableObject
{
    [SerializeField]
    private float _moistureLevel;
    public float MoistureMax;
    
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

    public List<Nozzle> Nozzles;
    public int Coins, TotalCoins;

    [SerializeField]
    private string InScene;

    public string CurrentScene { get => InScene; set => InScene =value; }

    [SerializeField]
    private Vector3 LocationInScene = Vector3.zero; 

    public Vector3 PositioninScene { get => LocationInScene; set => LocationInScene = value; }

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
