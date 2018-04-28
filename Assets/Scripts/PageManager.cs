using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour {

    public static PageManager instance;
    public GameObject[] prefabs;
    public GameObject CampaignView;
    public GameObject currrentPage = null;
    public GameObject viewport;

    PinchableScrollRect scrollRect;
    GameObject pinManager;
    string openPage;

    /// <summary>
    /// Called on creation
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Called after awake methods are called
    /// </summary>
    void Start(){
        scrollRect = GameObject.FindObjectOfType<PinchableScrollRect>();
        pinManager = GameObject.Find("PinManager");
        pinManager.SetActive(false);
    }

    /// <summary>
    /// Called every frame, checks if the back button on android is called and returns to the file view
    /// </summary>
    void Update() {
        if(currrentPage != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeletePage();
                SetActiveCampaignView(true);
            }
        }    
        
    }

    /// <summary>
    /// Delete the current page, called when returning to the fileview
    /// </summary>
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

    /// <summary>
    /// Load a page, called by a campaign file
    /// </summary>
    /// <param name="file">File to load a page for</param>
    public void SwitchPage(CampaignFile file)
    {
      
        DeletePage();
        Debug.Log("Switching to page: " + file.GetFileName());

        string fullPath = file.GetCampaign().GetCampaignName() + "/" + file.GetFileName() +"." + file.GetExtension();
        fullPath = SerializationManager.CreatePath(fullPath);

        //Get the file extension to determine what file type to display
        switch (file.GetExtension())
        {
            case "sbd":
                {
                    pinManager.SetActive(false);
                    currrentPage = Instantiate(prefabs[0], viewport.transform);
                    StatBlockUIData uiData = (StatBlockUIData)SerializationManager.LoadObject(fullPath);

                    currrentPage.GetComponent<StatBlockForm>().BuildPage(uiData);
                    currrentPage.GetComponent<StatBlockForm>().fullPath = fullPath;
                    currrentPage.GetComponent<StatBlockForm>().campaign = file.GetCampaign().GetCampaignName();
                    break;
                }
            case "map":
                {
                    pinManager.SetActive(true);
                    break;
                }
            default:
                {
                    Debug.Log(file.GetExtension() + " is not a proper extension!");
                    return;
                } 
        }

        scrollRect.enabled = true;
        scrollRect.content = currrentPage.GetComponent<RectTransform>();

        SetActiveCampaignView(false);
    }

    /// <summary>
    /// Toggle whether a page shoud be displayed or not.
    /// </summary>
    /// <param name="setActive">Determines the new state of campaign view</param>
    public void SetActiveCampaignView(bool setActive)
    {
        CampaignView.SetActive(setActive);
    }
}
