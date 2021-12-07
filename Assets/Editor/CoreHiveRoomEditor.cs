using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(QueenRoom))]
public class CoreHiveRoomEditor : Editor
{
  
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // button delete the hive room
        if (GUILayout.Button("Spawn Bug"))
        {
            QueenRoom qr = (QueenRoom)target;
            qr.SpawnBug();
        }

        if (GUILayout.Button("Send Bugs to collect"))
        {
            QueenRoom qr = (QueenRoom)target;
            qr.SendToCollect();
        }

        if (GUILayout.Button("Recall Bugs"))
        {
            QueenRoom qr = (QueenRoom)target;
            qr.RecallBugs();
        }

    }
}
