using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds Level Status infromation
/// How many coins are in the world/How many coins were collected
/// Location of each of the coins
/// What wells have been filled 
/// How many times the player has entered the world 
/// etc
/// </summary>
[CreateAssetMenu(fileName = "New Persistant Level", menuName = "LevelSO/Level", order = 200)]
public class LevelStatusSo : ScriptableObject
{

    public List<bool> CollectablesCollected = new List<bool>();
    
    public List<bool> WellsFilled = new List<bool>();

    public List<bool> TanksFilled = new List<bool>();

    public List<bool> Dialogue = new List<bool>(); 


    public float TimesEntered; 
}
