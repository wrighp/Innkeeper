using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapForm : PageObject {

    public GameObject pin;

    /// <summary>
    /// Build a page based on the set of give data
    /// </summary>
    /// <param name="data"></param>
    public override void BuildPage(PageData data)
    {
        MapData uiData = (MapData)data;

        //Add pins to the map
        foreach (PinData p in uiData.pins)
        {
            GameObject tmp = Instantiate(pin, transform);
            tmp.transform.position = p.position;
        }
    }
}

[Serializable]
public class MapData : PageData
{
    public string imagePath;
    public PinData[] pins;

    /// <summary>
    /// Constructor, create a new set of map data to display
    /// </summary>
    /// <param name="g">Game object to serialize data from</param>
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
    /// <summary>
    /// Serialize a pin into its color and location
    /// </summary>
    /// <param name="g">Game object to serialize</param>
    public PinData(GameObject g)
    {
        //Take pin and convert to save data
        RectTransform rect = g.GetComponent<RectTransform>();
        position = rect.position;
        color = (Color32)g.GetComponent<Image>().color;
    }
}