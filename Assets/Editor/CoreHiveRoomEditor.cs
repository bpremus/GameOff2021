using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(QueenRoom))]
public class CoreHiveRoomEditor : Editor
{
  
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // button delete the hive room
        if (GUILayout.Button("Send bugs to new room"))
        {

        }

    }
}
