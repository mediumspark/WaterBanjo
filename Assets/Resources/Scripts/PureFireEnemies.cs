using UnityEngine;
using Pathfinding;

/// <summary>
/// These Enemies are purely made of fire and can be damaged by water
/// </summary>
public class PureFireEnemies : MeleeEnemy
{
    public ParticleSystem EnemyParticles;
    int BaseParticleMax;

    protected override void Awake()
    {
        base.Awake();
        AIDest = GetComponent<AIDestinationSetter>(); 
        EnemyParticles = GetComponentInChildren<ParticleSystem>();
        BaseParticleMax = EnemyParticles.main.maxParticles;
        isElemental = true; 
    }

    protected override void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("PlayerWater"))
        {
            Destroy(other);

            var main = EnemyParticles.main;
            if (CurrHealth > 0)
            {
                CurrHealth -= 5;
                main.maxParticles = BaseParticleMax * (CurrHealth / MaxHealth);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
