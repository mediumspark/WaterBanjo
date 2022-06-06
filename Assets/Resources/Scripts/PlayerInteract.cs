using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float InteractRange;
    public LayerMask InteractableLayers;

    static Collider[] InteractablesInRange;

    private void Update()
    {
        InteractablesInRange = Physics.OverlapSphere(transform.position, InteractRange, InteractableLayers);
    }

    public static void Interact()
    {
        try
        {
            InteractablesInRange[0].GetComponent<IInteractable>().OnInteract();
        }
        catch { Debug.LogWarning($"Interactables in Range is empty"); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ICollectable collectable))
        {
            collectable.OnCollect(); 
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, InteractRange);
    }
}
