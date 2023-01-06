#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(YarnCommands))]
public class YarnCommandsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 


    }

    [MenuItem("GameObject/Yarn/YarnCommands Object", false, 10050)]
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
#endif