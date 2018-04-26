using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Campaign : MonoBehaviour {

    string campaignName;

	// Use this for initialization
	void Start () {
		
	}
	
    //Set name of folder on instantiation
    public void SetCampaignName(string name)
    {
        campaignName = name;
        GetComponentInChildren<InputField>().text = name;
    }

    public string GetCampaignName()
    {
        return campaignName;
    }


    //Change the name pf the folder if and when the user changes the name
	public void ChangeCampaignName(string name) {
        var oldPath = SerializationManager.CreatePath(campaignName);
        var newPath = SerializationManager.CreatePath(name);
        if (oldPath.Equals(newPath) || Directory.Exists(newPath))
        {
            //Rebuild in order to reset text back to original name
            FileManager.instance.ReloadCampaignTabs();
            return;
        }

        Debug.LogFormat("Folder: {0}\nRenaming to: {1}", oldPath, newPath);
        try
        {
            Directory.Move(oldPath, newPath);

            campaignName = name; //Will get destroyed and recreated anyway
            FileManager.instance.ReloadCampaignTabs();
        }
        catch(IOException ex)
        {
            Debug.LogException(ex);
        }
    }

    //Delete campaign
    public void DeleteCampaign() {
        SerializationManager.DeleteFolder(SerializationManager.CreatePath(campaignName),true);
        FileManager.instance.ReloadCampaignTabs();
    }
}
