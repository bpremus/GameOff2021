using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HiveCell))]
public class CellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (target == null) return;
        HiveCell hc = (HiveCell)target;
        if (hc == null) return;
        

        // button delete the hive room
        if (GUILayout.Button("Delete room"))
        {
            BuildManager bm = BuildManager.Instance;
            bm.SetCell(hc);
            bm.DestroyRoom();
        }

        if (GUILayout.Button("Create Cooridor"))
        {
            BuildManager bm = BuildManager.Instance;
            bm.SetCell(hc);
            bm.CreateCorridor(0);
        }
        if (GUILayout.Button("Create Room"))
        {
            BuildManager bm = BuildManager.Instance;
            bm.SetCell(hc);
            bm.CreateNewRoom(1);
        }
    }
}
