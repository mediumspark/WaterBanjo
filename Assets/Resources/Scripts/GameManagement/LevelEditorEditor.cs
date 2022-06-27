using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class LevelEditorEditor : Editor
{
    /// <summary>
    /// Create the ScriptableLevelReferenceIn the Scene 
    /// </summary>
    [MenuItem("GameObject/Level/Scriptable Object Reference", false, 50)]
    private static void CreateScriptableReference()
    {
        new GameObject("Level Reference").AddComponent<LevelScriptableRefference>();
    }
#endif
}