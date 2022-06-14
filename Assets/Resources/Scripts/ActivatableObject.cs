using UnityEngine;
/// <summary>
/// General Activation Objects like: Doors, Windows, Lights, Etc
/// Meant to be the final link in a chain of activatable objects
/// </summary>
public class ActivatableObject : MonoBehaviour, IActivateable
{
    public bool Activated = false; 

    public void OnActivation()
    {
        Activated = true;
        UpdateActivatedBool(Activated);
    }

    public void OnDeactivate()
    {
        Activated = false;
        UpdateActivatedBool(Activated);
    }

    public void UpdateActivatedBool(bool isActive)
    {
        try
        {
            GetComponentInChildren<Animator>().SetBool("Activated", Activated);
        }
        catch { }
    }
}
