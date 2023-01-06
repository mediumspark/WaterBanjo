using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemies are split into 
///   - Enemies made of pure fire 
///   - Enemies that use fire as a weapon
///   - Enemies that don't use fire
///   - Enemies that are currupted water
///   Some enemies use projectiles, 
///   Some are stationary, 
///   Some move
/// </summary>
public class Enemy : MonoBehaviour
{
    protected PlayerMovement Player; 

    protected bool _PlayerInRange
    {
        get
        {
            return Physics.CheckSphere(transform.position, PlayerDetectionRange, 1 << LayerMask.NameToLayer("Player"));
        }
    }


    [SerializeField]
    protected InitialPath Path;
    protected int PathIndex; 
    protected int MaxHealth, CurrHealth;

    public CharacterController CC; 
    public bool isElemental, isRanged;
    public float PlayerDetectionRange;
    public float speed;

    protected virtual void Awake()
    {
        //Temp
        MaxHealth = 20;
        CurrHealth = MaxHealth;
        Player = FindObjectOfType<PlayerMovement>();
        CC = GetComponent<CharacterController>(); 
    }

    protected virtual void OnParticleCollision(GameObject other)
    {

    }

    protected virtual void PatrolState()
    {

    }

    protected virtual void WhenEnemyInRange() { }

    protected void Update()
    {
        if (_PlayerInRange)
        {
            WhenEnemyInRange();
        }
        else if(Path.Path != null)
        {
            PatrolState(); 
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRange);
    }

    [System.Serializable]
    public struct InitialPath
    {
        public Transform Path;
    }
}

/// <summary>
/// Enemies that have something that they will strike with
/// Ranged Enemies (even unarmed) still have a weapon
/// </summary>
public class EnemyWithWeapon : Enemy
{
    protected void Attack() { }

    protected override void WhenEnemyInRange()
    {
        Attack(); 
    }
}
