using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HiveCorridor))]
public class CorridorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // button delete the hive room
        if (GUILayout.Button("Get a Bug from war room"))
        {
            HiveCorridor hr = (HiveCorridor)this.target;
            hr.GetBugsFromWarRoom();
        }
    }
}
