using UnityEngine;

public class Nozzle : ScriptableObject
{
    public int MoistureCost;
    public NozzleNames NZL;
    public GameObject Attack; 
    public bool Unlocked;
    public Sprite Sprite; 

    private void OnEnable()
    {
        name = NZL.ToString(); 
    }
}
