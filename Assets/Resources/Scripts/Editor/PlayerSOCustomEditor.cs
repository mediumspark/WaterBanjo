#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerSOCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var P = target as Player; 

        if(GUILayout.Button("Fill MoistureLevel"))
        {
            P.MoistureLevel = P.MoistureMax;
        }

        if (GUILayout.Button("Heal"))
        {
            P.Health = P.Hearts * 2; 
        }
    }
}
#endif