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

    public CharacterController CC; 
    protected int MaxHealth, CurrHealth;
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

    protected virtual void WhenEnemyInRange() { }

    protected void Update()
    {
        if (_PlayerInRange)
        {
            WhenEnemyInRange(); 
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRange);
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
