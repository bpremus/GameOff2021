using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HiveCell))]
public class CellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HiveCell hc = (HiveCell)target;
        if (target == null) return;

        // button delete the hive room
        if (GUILayout.Button("Delete room"))
        {

        }

        if (GUILayout.Button("Create Cooridor"))
        {
           // place room
        }
        if (GUILayout.Button("Create Room"))
        {
            // place room
        }
    }
}
