using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Game object that references the scriptable object for each level
/// </summary>
public class LevelScriptableReference : MonoBehaviour
{
    [SerializeField]
    private LevelStatusSo _levelInfo;

    //Represent variables from the scriptable objects
    [SerializeField]
    List<bool> CoinsCollected = new List<bool>();
    [SerializeField]
    List<bool> WellsFilled = new List<bool>();
    [SerializeField]
    List<bool> TanksFilled = new List<bool>();
    
    public Vector3 StartLocation; 

    //Cache for the Collectables currently in scene
    Coin[] CoinCache;
    //Cached Wells currently in scene
    Well[] WellCache;
    //Cached Tanks in scene if any
    WaterTank[] TankCache; 

    private void Awake()
    {
        _levelInfo.TimesEntered++;
        SetCache();
    }

    /// <summary>
    /// Creates and sets the cache/interacted values in the scriptableobject and in the cache for the reference
    /// </summary>
    public void SetCache()
    {

        CoinCache = FindObjectsOfType<Coin>().ToArray();
        WellCache = FindObjectsOfType<Well>().ToArray();
        TankCache = FindObjectsOfType<WaterTank>().ToArray(); 

        CoinsCollected = _levelInfo.CollectablesCollected;
        WellsFilled = _levelInfo.WellsFilled;
        TanksFilled = _levelInfo.TanksFilled; 

        //If Level scriptable object is empty on start then there is no reason to check
        if (_levelInfo.CollectablesCollected.Count == CoinCache.Length && _levelInfo.WellsFilled.Count == WellCache.Length)
        {
            int Cindex = 0;
            while (Cindex < CoinsCollected.Count)
            {
                CoinCache[Cindex].gameObject.SetActive(!CoinsCollected[Cindex]);
                Cindex++;
            }

            int Windex = 0;
            while (Windex < WellsFilled.Count)
            {
                if (WellsFilled[Windex])
                    WellCache[Windex].WaterInWell = WellCache[Windex].WaterCapacity;
                Windex++;
            }    
            
            int Tindex = 0;
            while (Tindex < WellsFilled.Count)
            {
                if (WellsFilled[Tindex])
                    WellCache[Tindex].WaterInWell = WellCache[Tindex].WaterCapacity;
                Tindex++;
            }
        }
        //clears lists on the so and then adds the cache to the lists. 
        else
        {
            _levelInfo.WellsFilled.Clear();
            _levelInfo.CollectablesCollected.Clear(); 

            foreach (Coin Go in CoinCache)
                _levelInfo.CollectablesCollected.Add(!Go.gameObject.activeSelf);

            foreach (Well wll in WellCache)
                _levelInfo.WellsFilled.Add(wll.WaterInWell < wll.WaterCapacity);       
            
            foreach (WaterTank Tank in TankCache)
                _levelInfo.WellsFilled.Add(Tank.CurrentWaterLevel < Tank.MaxWaterLevel);
        }
    }

    /// <summary>
    /// For Hub Areas: So that players can spawn in the area where they were last present
    /// </summary>
    /// <param name="Spawn"></param>
    public void SetSpawnInLocation(Vector3 Spawn)
    {
        try
        {
            HubLevel _hlevelInfo = _levelInfo as HubLevel;

            _hlevelInfo.SpawnInLocation = Spawn;

            _levelInfo = _hlevelInfo;
        }
        catch
        {
            Debug.LogError($"Tried to cast standard level: {_levelInfo.name} as Hublevel. Standard levels do not have flexible on start spawn locations");
        }
    }


    public void CoinCollected(Coin coin)
    {
        CoinsCollected[CoinCache.ToList().IndexOf(coin)] = true;
    }

    public void WellFilled(Well Well)
    {
        WellsFilled[WellCache.ToList().IndexOf(Well)] = true; 
    }

    public void WaterTankFilled(WaterTank Tank)
    {
        TanksFilled[TankCache.ToList().IndexOf(Tank)] = true; 
    }

    private void OnDisable()
    {
        _levelInfo.CollectablesCollected = CoinsCollected;
        _levelInfo.WellsFilled = WellsFilled;
        _levelInfo.TanksFilled = TanksFilled; 
    }

}
