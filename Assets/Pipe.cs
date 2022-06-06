using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnInteract();
}

public class Pipe : MonoBehaviour, IInteractable
{

    public void TeleportPlayer()
    {
        PlayerControls.PlayerMovement.GetComponent<CharacterController>().enabled = false; 
        PlayerControls.PlayerMovement.transform.position = transform.GetChild(0).position;
        PlayerControls.PlayerMovement.GetComponent<CharacterController>().enabled = true;
    }

    public void OnInteract()
    {
        GetComponentInParent<PipeWork>().TransportPlayerBetweenPipes(transform.parent.GetChild(0).name == name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.GetChild(0).position, 0.5f);
    }
}
