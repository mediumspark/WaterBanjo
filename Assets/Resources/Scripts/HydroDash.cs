using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroDash : MonoBehaviour
{
    PlayerMovement PM;
    private void Awake()
    {
        PM = FindObjectOfType<PlayerMovement>();
        PM.isDashing = true; 
        StartCoroutine(KillAbility());
    }

    private void OnDisable()
    {
        PM.isDashing = false; 
    }

    public IEnumerator KillAbility()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject); 
    }
}
