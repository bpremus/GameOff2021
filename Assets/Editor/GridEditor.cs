using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HiveGenerator))]
public class GridEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
  
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

        HiveGenerator hc = (HiveGenerator)target;
        if (target == null) return;

        // delete the hive and start again
        if (GUILayout.Button("Delete Hive"))
        {
            hc.DeleteGrid();
        }
        // button to generate base grid 
        if (GUILayout.Button("Generate Hive"))
        {
            Debug.Log("generating a new hive layout");
            hc.GenerateStaticGrid();
        }
    }
}
