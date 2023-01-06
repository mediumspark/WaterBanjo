using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;


public class YarnCommands : MonoBehaviour
{
    public CharacterList CharacterList; 

    [SerializeField]
    private PlayerAnimations Player;

    private static DialogueRunner _dRunner;
    private static YarnProject _yProject; 

    private void Awake()
    {
        Player = FindObjectOfType<PlayerAnimations>();
        _dRunner = FindObjectOfType<DialogueRunner>();
        _yProject = _dRunner.yarnProject;
        

        _dRunner.AddCommandHandler("Start", ConversationStarted); 
        _dRunner.AddCommandHandler("End", ConversationEnded); 
    }

    public static void StartDialogue(string DialogueToStart) => _dRunner.StartDialogue(DialogueToStart);

    private void ConversationStarted() => Player.StopPlayer();

    private void ConversationEnded() => Player.ContinuePlayer();
}
