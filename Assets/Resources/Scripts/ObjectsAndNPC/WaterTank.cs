using UnityEngine;
using System.Collections.Generic;
using System.Collections;


/// <summary>
/// Activatable Object meant to be filled with water and then activates another activatable when completely full
/// </summary>
public class WaterTank : MonoBehaviour, IActivateable
{
    [SerializeField]
    private ActivatableObject _activatedWhenTankFull; 

    [SerializeField]
    private float _currentTank, _tankGoal, _tankGainPercentage;

    [SerializeField]
    private GameObject _waterLevel;

    public float MaxWaterLevel => _tankGoal;
    public float CurrentWaterLevel => _currentTank; 


    public void OnActivation()
    {      
        _currentTank = _currentTank < _tankGoal ? _currentTank + _tankGainPercentage : OnTankFull();
        UpdateWaterLevel(); 
    } 

    private void UpdateWaterLevel() => _waterLevel.transform.localScale = new Vector3(_waterLevel.transform.localScale.x,
                (_currentTank / _tankGoal) * 0.38f % 0.39f, _waterLevel.transform.localScale.z);


    private float OnTankFull()
    {
        _activatedWhenTankFull.OnActivation(); 
        return _tankGoal; 
    }

    private void OnTankEmpty() => _activatedWhenTankFull.OnDeactivate();


    private IEnumerator DrainTank(float TankDrainDestination, float waitTime)
    {
        if(_currentTank > TankDrainDestination)
        {
            _currentTank -= 1.0f;
            UpdateWaterLevel(); 
            yield return new WaitForSeconds(waitTime);

            StartCoroutine(DrainTank(TankDrainDestination, waitTime));
        }
        else {
            OnTankEmpty(); 
        }

    }

    public Coroutine Drain()
    {
        return StartCoroutine(DrainTank(0, 0.5f));
    }
}
