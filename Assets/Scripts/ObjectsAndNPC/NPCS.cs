using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCS : MonoBehaviour, IInteractable
{
    [SerializeField]
    private bool hasDialogue;
    [SerializeField]
    string DialogueName; 

    public void OnInteract()
    {
        YarnCommands.StartDialogue(DialogueName); 
    }
}
