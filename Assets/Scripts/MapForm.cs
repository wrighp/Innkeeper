using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapForm : PageObject {

    public GameObject pin;

    public override void BuildPage(PageData data)
    {
        MapData uiData = (MapData)data;

        foreach (PinData p in uiData.pins)
        {
            GameObject tmp = Instantiate(pin, transform);
            tmp.transform.position = p.position;
        }

        //GetComponent<Image>().sprite

    }

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