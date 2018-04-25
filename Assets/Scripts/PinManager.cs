using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PinManager : MonoBehaviour {

    public GameObject pinPrefab;
    public GameObject dragIcon;

    GameObject drawnImage;
    bool heldDown = false;

    public void Update()
    {
        dragIcon.transform.position = Input.mousePosition;
    }

    public void Drag(GameObject obj)
    {
        Image dragImage = dragIcon.GetComponent<Image>();
        dragImage.color = obj.GetComponent<Image>().color;
        dragImage.enabled = true;

        if (obj.name != "Button")
        {
            obj.GetComponent<Image>().enabled = false;
        }

        heldDown = true;
    }

    public void Release(GameObject obj)
    {
        if (!heldDown)
        {
            return;
        }
        RectTransform rt = GameObject.FindObjectOfType<PinchableScrollRect>().content;
        Image dragImage = dragIcon.GetComponent<Image>();
        dragImage.enabled = false;
        heldDown = false;

        GameObject canvas = GameObject.Find("Canvas");

        GameObject newPin = Instantiate(pinPrefab, canvas.transform as RectTransform);
        newPin.transform.position = Input.mousePosition;
        newPin.transform.SetParent(rt.transform);
        newPin.transform.localScale = Vector3.one;
        newPin.GetComponent<Image>().color = dragImage.color;

        if (obj.name != "Button")
        {
            newPin.GetComponent<Pin>().reference = obj.GetComponent<Pin>().reference;
            Destroy(obj);
        }

        if (!rt.rect.Contains(Input.mousePosition))
        {
            Destroy(newPin);
        }
    }
}