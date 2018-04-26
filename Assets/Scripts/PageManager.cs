using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour {

    public static PageManager instance;

    public GameObject[] prefabs;

    public GameObject CampaignView;

    public GameObject currrentPage = null;

    PinchableScrollRect scrollRect;
    GameObject pinManager;
    public GameObject viewport;

    string openPage;

    private void Awake()
    {
        instance = this;
    }

    void Start(){
        scrollRect = GameObject.FindObjectOfType<PinchableScrollRect>();
        pinManager = GameObject.Find("PinManager");
        pinManager.SetActive(false);
    }

    void Update() {

            
        
    }

    public void DeletePage()
    {
        scrollRect.enabled = false;
        scrollRect.content = null;
        if (viewport.transform.childCount == 0)
        {
            return;
        }
        //Get Viewport
        GameObject.Destroy(viewport.transform.GetChild(0).gameObject);
    }

    public void SwitchPage(CampaignFile file)
    {
      
        DeletePage();
        Debug.Log("Switching to page: " + file.GetFileName());

        string fullPath = file.GetCampaign().GetCampaignName() + "/" + file.GetFileName() +"." + file.GetExtension();
        fullPath = SerializationManager.CreatePath(fullPath);

        switch (file.GetExtension())
        {
            case "sbd":
                
                pinManager.SetActive(false);
                currrentPage = Instantiate(prefabs[0], viewport.transform);
                StatBlockUIData uiData = (StatBlockUIData)SerializationManager.LoadObject(fullPath);

                currrentPage.GetComponent<StatBlockForm>().BuildPage(uiData);
                currrentPage.GetComponent<StatBlockForm>().fullPath = fullPath;
                currrentPage.GetComponent<StatBlockForm>().campaign = file.GetCampaign().GetCampaignName();
                break;
            case "map":
                pinManager.SetActive(true);
                break;
            default: 
                Debug.Log(file.GetExtension() + " is not a proper extension!"); 
                return; 
                break; 
        }

        

        scrollRect.enabled = true;
        scrollRect.content = currrentPage.GetComponent<RectTransform>();

        SetActiveCampaignView(false);
    }

    public void SetActiveCampaignView(bool setActive)
    {
        CampaignView.SetActive(setActive);
    }
}
