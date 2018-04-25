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

[Serializable]
public class MapData : PageData
{
    public string imagePath;
    public PinData[] pins;

    public MapData(GameObject g)
    {
        //Loop through transform for pins and create pindata
        List<PinData> lP = new List<PinData>();
        foreach (Pin p in g.GetComponentsInChildren<Pin>())
        {
            lP.Add(new PinData(p.gameObject));
        }
    }

}

[Serializable]
public class PinData
{
    public string referencePath;
    public Color32 color;
    public Vector3 position;

    public PinData(GameObject g)
    {
        //Take pin and convert to save data
        RectTransform rect = g.GetComponent<RectTransform>();
        position = rect.position;
        color = (Color32)g.GetComponent<Image>().color;
    }
}