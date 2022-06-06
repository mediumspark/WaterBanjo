using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroJump : MonoBehaviour
{
    PlayerMovement PM;
    private void Awake()
    {
        PM = FindObjectOfType<PlayerMovement>();
        PM.Launch = true;
        StartCoroutine(KillAbility());
    }

    private void OnDisable()
    {
        PM.Launch = false; 
    }

    public IEnumerator KillAbility()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
