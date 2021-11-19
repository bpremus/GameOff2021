using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WarRoom))]
public class WarRoomEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // button delete the hive room
        if (GUILayout.Button("Get a Bug from hive"))
        {
            WarRoom hr = (WarRoom)this.target;
            hr.GetBugsFromHive();
        }
    }
}

