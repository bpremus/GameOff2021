using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HarversterRoom))]
public class HarvesterRoomEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // button delete the hive room
        if (GUILayout.Button("Get a Bug from hive"))
        {
            HarversterRoom hr = (HarversterRoom)this.target;
            hr.GetBugsFromHive();
        }

    }
}
