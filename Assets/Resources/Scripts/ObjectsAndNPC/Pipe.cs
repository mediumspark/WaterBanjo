using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        GetComponentInParent<PipeNetwork>().TransportPlayerBetweenPipes(transform.parent.GetChild(0).name == name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.GetChild(0).position, 0.5f);
    }
}
