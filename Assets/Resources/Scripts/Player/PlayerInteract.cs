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

    private PlayerAnimations Animations;

    private void Awake()
    {
        CC = GetComponent<CharacterController>();
        Animations = GetComponent<PlayerAnimations>(); 
    }

    private void Update()
    {
        InteractablesInRange = Physics.OverlapSphere(transform.position, InteractRange, InteractableLayers);
        bool hit = Physics.CheckSphere(transform.position, CC.radius + 0.5f, DamageLayers);
        if (hit)
            PlayHurt();

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Instakill"))
            StartCoroutine(PlayerScriptableReference.PlayerSO.OnDeath());
    }

    /// <summary>
    /// Pass AnimatorController Hurt through Playermovement
    /// Player goes invol and is launched back a little bit
    /// </summary>
    public void PlayHurt()
    {
        Animations.AniHurt();
    }

    public void TakeDamage(int damage)
    {
        PlayerScriptableReference.PlayerSO.TakeDamage(damage);
    }

    /// <summary>
    /// Gives the player Iframes based on its duration
    /// </summary>
    /// <param name="On"> Used in the Animator which does not allow for booleans in methods. > 0 is true, <= 0 is false </param>
    public void IFrames(int On = 0)
    {
        bool IFramesOn = On > 0;
        Animations.Invol(IFramesOn);
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
            if(collectable is Coin)
            {
                Animations.PlayCoinCollected(); 
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, InteractRange);
    }
}
