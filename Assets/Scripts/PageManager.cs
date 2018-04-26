using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour {

    public static PageManager instance;

    public GameObject[] prefabs;

    public GameObject currrentPage = null;

    PinchableScrollRect scrollRect;
    GameObject pinManager;
    GameObject viewport;

    string openPage;

    private void Awake()
    {
        instance = this;
    }

    void Start(){
        scrollRect = GameObject.FindObjectOfType<PinchableScrollRect>();
        pinManager = GameObject.Find("PinManager");
        viewport = GetComponentInChildren<Mask>().gameObject;
    }

    void Update() {

            
        
    }

    public void DeletePage()
    {
        if(viewport.transform.childCount == 0)
        {
            return;
        }
        //Get Viewport
        GameObject.Destroy(viewport.transform.GetChild(0));
    }

    public void SwitchPage(CampaignFile file)
    {
        DeletePage();
        string fullPath;

        switch (file.GetExtension())
        {
            case "sbd":
                /*
                pinManager.SetActive(false);
                currrentPage = Instantiate(prefabs[0], viewport.transform);
                StatBlockUIData uiData = (StatBlockUIData)SerializationManager.LoadObject(fullPath);

                currrentPage.GetComponent<StatBlockForm>().BuildPage(uiData);
                */
                break;
            case "map":
                pinManager.SetActive(true);
                break;
        }

        

        scrollRect.enabled = true;
        scrollRect.content = currrentPage.GetComponent<RectTransform>();
        

       

    }

}
