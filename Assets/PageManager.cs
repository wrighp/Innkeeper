using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour {

    PinchableScrollRect scrollRect;
    GameObject currrentPage = null;

    void Start(){
        scrollRect = GameObject.FindObjectOfType<PinchableScrollRect>();
    }

    void Update() {
        if(NetworkHandler.instance.pages.Count > 0 && currrentPage == null) {
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

}
