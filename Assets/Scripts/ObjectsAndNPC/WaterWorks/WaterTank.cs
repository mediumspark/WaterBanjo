using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events; 
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    [SerializeField]
    private GameObject _drainPipe;

    [SerializeField]
    private ParticleSystem _WaterPrefab; 

    public float MaxWaterLevel => _tankGoal;
    public float CurrentWaterLevel => _currentTank;

    public OnDrainItem OnDrain; 


    public void OnActivation()
    {      
        _currentTank = _currentTank < _tankGoal ? _currentTank + _tankGainPercentage : OnTankFull();
        UpdateWaterLevel(); 
    } 

    private void UpdateWaterLevel() => _waterLevel.transform.localScale = new Vector3(_waterLevel.transform.localScale.x,
                (_currentTank / _tankGoal) * 0.38f % 0.39f, _waterLevel.transform.localScale.z);


    private float OnTankFull()
    {
        if(_activatedWhenTankFull != null)
            _activatedWhenTankFull.OnActivation();
       

        return _tankGoal;

    }

    private void OnTankEmpty()
    {
        if (OnDrain != null)
            OnDrain.OnDeactivate.Invoke();
        _activatedWhenTankFull.OnDeactivate();
    }

    private IEnumerator DrainTank(float TankDrainDestination, float waitTime)
    {
        if(_currentTank > TankDrainDestination)
        {
            Instantiate(_WaterPrefab, _drainPipe.transform); 
            _currentTank -= 1.0f;
            UpdateWaterLevel();
           
            if(OnDrain!=null)
                OnDrain.OnActivation.Invoke(); 

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_activatedWhenTankFull != null)
        {
            Vector3 manpos = transform.position;
            Vector3 pointpos = _activatedWhenTankFull.GetComponent<Collider>().bounds.center;
            float halfheight = (manpos.y - pointpos.y) * 0.5f;
            Vector3 offset = Vector3.up * halfheight;

            Handles.DrawBezier(manpos, pointpos, manpos - offset, pointpos - offset, Color.white, EditorGUIUtility.whiteTexture, 0.5f);
        }
    }
#endif
}