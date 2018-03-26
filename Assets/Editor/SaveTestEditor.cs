using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SaveTest))]
public class SaveTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveTest test = (SaveTest)target;
        if (GUILayout.Button("Save"))
        {
            test.SaveData();
        }
        if(GUILayout.Button("Clear Data"))
        {
            test.ClearData();
        }
        if (GUILayout.Button("Load"))
        {
            test.LoadData();
        }
        if(GUILayout.Button("Delete File"))
        {
            test.DeleteFile();
        }
    }
}