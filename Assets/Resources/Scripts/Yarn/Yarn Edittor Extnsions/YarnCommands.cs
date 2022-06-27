using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Yarn.Unity;


public class YarnCommands : MonoBehaviour
{
    public CharacterList CharacterList;

    [SerializeField]
    private PlayerAnimations Player;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerAnimations>();
    }

    [YarnCommand("Start")]
    public void ConversationStarted() 
    {
        Player.StopPlayer();
    }

    [YarnCommand("End")]
    public void ConversationEnded()
    {
        Player.ContinuePlayer();
    }
}

[CustomEditor(typeof(YarnCommands))]
public class YarnCommandsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 


    }

    [MenuItem("GameObject/Yarn/CharactersList Object", false, 10050)]
    private static void CreateNewDialogueObject()
    {
        YarnCommands go = new GameObject("Dialogue").AddComponent<YarnCommands>();


        go.CharacterList = Resources.Load("ScriptableObjects/CharacterList") as CharacterList;

        if (go.CharacterList == null)
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CharacterList>(), "Assets/Resources/ScriptableObjects/CharacterList.asset");
            go.CharacterList = Resources.Load("ScriptableObjects/CharacterList") as CharacterList;
        }
    }
}
