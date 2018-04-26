using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class PageObject : MonoBehaviour
{
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