using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float InteractRange;
    public LayerMask InteractableLayers;
    public LayerMask DamageLayers; 

    static Collider[] InteractablesInRange;

    private CharacterController CC; // Cached for use in Collision Check

    private void Awake()
    {
        CC = GetComponent<CharacterController>();
    }

    private void Update()
    {
        InteractablesInRange = Physics.OverlapSphere(transform.position, InteractRange, InteractableLayers);
        bool hit = Physics.CheckSphere(transform.position, CC.radius, DamageLayers);
        if (hit) PlayerControls.PlayerMovement.Hurt();

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Instakill"))
            StartCoroutine(PlayerScriptableReference.PlayerSO.OnDeath());
    }

    public void IFrames(int On = 0)
    {
        bool IFramesOn = On > 0;
        if (IFramesOn)
        {
            GetComponent<Collider>().enabled = false;
            PlayerControls.PlayerMovement.Stop(); 
        }
        else
        {
            GetComponent<Collider>().enabled = true;
        }
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
        Gizmos.DrawWireSphere(transform.position, CC.radius);
    }
}
