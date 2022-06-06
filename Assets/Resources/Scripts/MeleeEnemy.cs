using UnityEngine;
using Pathfinding; 

/// <summary>
/// Enemies that don't have hands are forced to chase the player 
/// </summary>
public class MeleeEnemy : Enemy
{
    protected AIDestinationSetter AIDest; 

    protected override void WhenEnemyInRange()
    {
        GetComponent<AIPath>().maxSpeed = speed > 0 ? speed : 5; 
        AIDest.target = Player.transform;  
    }
}