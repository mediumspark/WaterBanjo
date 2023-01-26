using System.Collections.Generic; 
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

    protected override void PatrolState()
    {
        if (GetComponent<AIPath>().reachedDestination)
        {
            PathIndex = PathIndex < Path.Path.childCount ? PathIndex + 1 : 0; 
        }
        AIDest.target = Path.Path.GetChild(PathIndex);

    }
}