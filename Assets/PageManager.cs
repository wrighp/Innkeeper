using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour {

    PinchableScrollRect scrollRect;
    public GameObject currrentPage = null;
    GameObject pinManager;

    void Start(){
        scrollRect = GameObject.FindObjectOfType<PinchableScrollRect>();
        pinManager = GameObject.Find("PinManager");
    }

    void Update() {
        if (NetworkHandler.instance == null)
        {
            return;
        }
        if (NetworkHandler.instance.pages.Count > 0 && currrentPage == null) {
            foreach (GameObject page in NetworkHandler.instance.pages.Values)
            {
                currrentPage = page;
                break;
            }
            currrentPage.SetActive(true);
            scrollRect.enabled = true;
            scrollRect.content = currrentPage.GetComponent<RectTransform>();
        }
    }

    public void SwitchPage()
    {
        pinManager.SetActive(currrentPage.GetComponent<Image>() != null);
        
    }

}
