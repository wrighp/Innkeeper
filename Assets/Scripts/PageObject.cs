using UnityEngine;
using System;

public abstract class PageObject : MonoBehaviour
{
    public string campaign;
    public abstract void BuildPage(PageData data);
}

[Serializable]
public abstract class PageData
{
    public string name;
    //On/before/after serialize/deserialize
    //public abstract void OnAfterDeserialize();
    //public abstract void OnBeforeSerialize();
}