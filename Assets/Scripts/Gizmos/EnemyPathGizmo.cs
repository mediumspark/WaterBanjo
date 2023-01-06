#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class EnemyPathGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {

            Vector3 manpos = transform.GetChild(i).position;
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.5f);
            Gizmos.color = Color.red;

            if (i + 1 < transform.childCount)
            {
                Vector3 pointpos = transform.GetChild(i + 1).position; 
                float halfheight = (manpos.y - pointpos.y) * 0.5f;
                Vector3 offset = Vector3.up * halfheight;

                Handles.DrawBezier(manpos, pointpos, manpos - offset, pointpos - offset, Color.red, EditorGUIUtility.whiteTexture, 0.5f);
            }
        }
    }

}
#endif