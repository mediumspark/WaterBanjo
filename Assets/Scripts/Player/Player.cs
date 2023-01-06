using System.Collections.Generic;
using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Saveslot",menuName = "Save Slot",order = 20)]
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

    [SerializeField]
    private int _TotalHearts; 
    public int Hearts { get => _TotalHearts; 
        set { _TotalHearts = value;
        }//Whenever a new heart is gained or a heart is lost it needs to update the amount of hearts on the UI
    }

    [SerializeField]
    private int _health; //Each heart is two health 
    public int Health { get => _health;
        set {
            _health = value;
            try
            {
                PlayerCanvas.UpdateHealth();
            }
            catch { }
        }//Whenever damage is taken half of the last heart that is not the color of damage needs to be changed to the color that indicates damage
    }

    public List<Nozzle> Nozzles;
    public int Coins, TotalCoins;

    public string RespawnScene;
    public Vector3 RespawnPosition;

    [SerializeField]
    public bool Dead = false; 


    public IEnumerator OnDeath()
    {
        if (!Dead)
        {
            Dead = true;
            PlayerControls.PlayerMovement.GetComponent<PlayerAnimations>().AniHurt();
            PlayerScriptableReference.PlayLevelLoad(); 

            //Fade to black Transition
            yield return new WaitForSeconds(0.15f);
            Refresh(); 

            //Load respawn scene
            SceneManager.LoadScene(RespawnScene);
        }
    }


    public void Refresh()
    {
        Health = _TotalHearts * 2; 
        MoistureLevel = MoistureMax;
        Dead = false; 
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

    public void TakeDamage(int Damage)
    {
        Health -= Damage; 
        if (Health <= 0)
        {
           PlayerControls.PlayerMovement.StartCoroutine( OnDeath()); 
        }
    }

    public void Reset()
    {
        InScene = SceneManager.GetSceneByBuildIndex(1).name;

        MoistureMax = 1000.0f;
        _moistureLevel = 0;
        Coins = 0;
        TotalCoins = 0;
        Dead = false;

        RespawnPosition = Vector3.zero;
        RespawnScene = "";

        Hearts = 3;
        Health = Hearts * 2; 

        Nozzles.Clear();

        Nozzles.Add(Resources.Load<Nozzle>("Nozzles/Spurt"));
        Nozzles.Add(Resources.Load<Nozzle>("Nozzles/Dash"));
    }
}
