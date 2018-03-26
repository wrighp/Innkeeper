using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveTest : MonoBehaviour
{
    [Serializable]
    public class SaveTestData
    {
        public string text;
        public int num;
    }

    public SaveTestData saveData;

    public State state;
    public enum State { Save, Load, Delete }
    // Use this for initialization
    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "test" + ".tst");

        switch (state)
        {
            case State.Save:
                    SerializationManager.SaveObject(path, saveData);
                    break;
            case State.Load:
                saveData = (SaveTestData)SerializationManager.LoadObject(path);
                break;
            case State.Delete:
                SerializationManager.DeleteFile(path);
                break;
        }
    }
    void Update()
    {

    }
}
