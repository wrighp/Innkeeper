using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour {


    public GameObject currrentPage = null;

    PinchableScrollRect scrollRect;
    GameObject pinManager;

    string openPage;

    void Start(){
        scrollRect = GameObject.FindObjectOfType<PinchableScrollRect>();
        pinManager = GameObject.Find("PinManager");
    }

    void Update() {

            //scrollRect.enabled = true;
            //scrollRect.content = currrentPage.GetComponent<RectTransform>();
        
    }

    public void SwitchPage()
    {
        //pinManager.SetActive(currrentPage.GetComponent<Image>() != null);
        
    }

}
