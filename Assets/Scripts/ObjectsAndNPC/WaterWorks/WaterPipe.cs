using UnityEngine;
/// <summary>
/// Wall mounted buttons that are triggered by being hit with water
/// </summary>
public class WaterPipe : WaterInteractableBehavior
{
    public GameObject Activateable; 

    protected override void OnWaterHit(GameObject other)
    {
        Activateable.GetComponent<IActivateable>().OnActivation(); //Activates when the button is hit with water
    }
}
