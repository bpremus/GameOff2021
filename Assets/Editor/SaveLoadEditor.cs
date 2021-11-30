using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveController))]
public class SaveLoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save game"))
        {
            SaveController save_controll = (SaveController) this.target;
            save_controll.Save();
        }

        if (GUILayout.Button("Clear hive"))
        {
            SaveController save_controll = (SaveController)this.target;
            save_controll.ClarHive();
        }

        if (GUILayout.Button("Load game"))
        {
            SaveController save_controll = (SaveController)this.target;
            save_controll.Load();
        }

    }
}
