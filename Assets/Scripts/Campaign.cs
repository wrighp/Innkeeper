using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Campaign : MonoBehaviour {

    public GameObject fileHolder;
    public GameObject campaignFilePrefab;

    string campaignName;

    /// <summary>
    /// Delete the file display prefabs from the scene, used when closing or reloading the tab
    /// </summary>
    public void DeleteFileTabs()
    {
        for (int i = 0; i < fileHolder.transform.childCount-1; ++i)
        {
            Destroy(fileHolder.transform.GetChild(i).gameObject);
        }
        //Modify the heights of layout elements to account for no children objects
        GetComponent<LayoutElement>().preferredHeight = 200;
        fileHolder.GetComponent<LayoutElement>().preferredHeight = 200;
    }

    /// <summary>
    /// Load all files currently in the campaign directory
    /// creating prefabs for each file, modify object heights to properly display files
    /// </summary>
    public void LoadFiles()
    {
        int count = 1;
        DeleteFileTabs();
        foreach (string s in FileManager.instance.GetSavedFilesFromCampaign(campaignName))
        {
            GameObject gO = Instantiate(campaignFilePrefab, fileHolder.transform);
            CampaignFile cf = gO.GetComponent<CampaignFile>();
            cf.SetFileName(s);
            cf.SetCampaignName(this);
            gO.transform.SetAsFirstSibling();
            count++;
        }
        GetComponent<LayoutElement>().preferredHeight = 200 + count * 200;
        fileHolder.GetComponent<LayoutElement>().preferredHeight = 200 + count * 200;
    }

    /// <summary>
    /// Event listener, determines whether a campaign should be expanded or closed
    /// </summary>
    public void ChangeVisibiliyOfContents()
    {
        if (!fileHolder.activeInHierarchy)
        {
            fileHolder.SetActive(true);
            LoadFiles();
        }
        else
        {
            DeleteFileTabs();
            fileHolder.SetActive(false);
        }
    }
	
    /// <summary>
    /// Set the name of the campaign for display purposes aty creation
    /// </summary>
    /// <param name="name">Name of the campaign</param>
    public void SetCampaignName(string name)
    {
        campaignName = name;
        GetComponentInChildren<InputField>().text = name;
    }

    /// <summary>
    /// Getter, returns the name of the current campaign
    /// </summary>
    /// <returns>Name of the campaign</returns>
    public string GetCampaignName()
    {
        return campaignName;
    }


    /// <summary>
    /// Event listener, change the name of the campaign when the users modifies the input field
    /// Chnages the underlaying directory name and reloads all the files under the new directory
    /// </summary>
    /// <param name="name">New name of the campaign</param>
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

    /// <summary>
    /// Event Listener, Create a DnD template file and add it to the directory
    /// </summary>
    public void AddDnDTemplate()
    {
        FileManager.instance.SaveTemplate("5e Template", campaignName);
        LoadFiles();
    }

    /// <summary>
    /// Event Listener, Create a Pathfinder template file and add it to the directory
    /// </summary>
    public void AddPathFinderTemplate()
    {
        FileManager.instance.SaveTemplate("Pathfinder Template", campaignName);
        LoadFiles();
    }

    /// <summary>
    /// Event Listener, Open the phone gallery and add the image to a map scene
    /// </summary>
    public void AddImage()
    {
        FileManager.instance.GetImage(campaignName);
        LoadFiles();
    }

    /// <summary>
    /// Event Listener, Delete the current campaign
    /// </summary>
    public void DeleteCampaign()
    {
        SerializationManager.DeleteFolder(SerializationManager.CreatePath(campaignName),true);
        FileManager.instance.ReloadCampaignTabs();
    }
}
