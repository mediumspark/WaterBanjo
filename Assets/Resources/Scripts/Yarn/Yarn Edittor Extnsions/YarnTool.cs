using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class YarnTool : Editor
{
    [MenuItem("GameObject/Yarn/Dialogue Prefab", false, 10050)]
    private static void CreateDialoguePrefab()
    {
        Instantiate(
            AssetDatabase.LoadAssetAtPath("Packages/dev.yarnspinner.unity/Prefabs/Dialogue System.prefab",
            typeof(GameObject))
            );
    }
}
