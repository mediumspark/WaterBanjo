using UnityEngine;

public class DrainButton : MonoBehaviour, IInteractable
{
    [SerializeField]
    private WaterTank AssignedWaterTank;

    public void OnInteract()
    {
        AssignedWaterTank.Drain();
    }
}
