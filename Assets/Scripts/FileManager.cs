using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System;

public class FileManager : MonoBehaviour
{
    public GameObject campaignPrefab;

    public static FileManager instance;


    //user chooses standard template to be saved as a modified template
    void SaveTemplate(string template)
    {
        TextAsset[] templateAssets = Resources.LoadAll<TextAsset>("Templates");
        Dictionary<string, string> templateAssetNames = new Dictionary<string, string>();
        for (int i = 0; i < templateAssets.Length; i++)
        {
            templateAssetNames.Add(templateAssets[i].name, templateAssets[i].text);
        }

        string path = SerializationManager.CreatePath(template + "-mod.txt");
        StatBlockUIData data = new StatBlockUIData();
        data.text = templateAssetNames[template];
        SerializationManager.SaveObject(path, data);
    }

    void PullAssetNames()
    {
        string assetPath = SerializationManager.CreatePath("");
        DirectoryInfo d = new DirectoryInfo(assetPath);
        Dictionary<string, string> assetFiles = new Dictionary<string, string>();
        foreach (var file in d.GetFiles())
        {
            assetFiles.Add(file.Name, file.FullName);
        }

        //Here, the user selects the name of the file they want displayed on the page
        string chosenFile = GameObject.Find("StatBlockInputField").GetComponent<Text>().text;
        SerializationManager.LoadObject(assetFiles[chosenFile]);
    }

    /// <summary>
    /// Get array of save folder names in SaveData path
    /// </summary>
    /// <returns>string[] of Folder names</returns>
    public string[] GetSavedCampaigns()
    {
        string folderPath = SerializationManager.CreatePath("");
        DirectoryInfo d = new DirectoryInfo(folderPath);
        //Sort directory by date created and then convert to a list of strings
        var stringList = d.GetDirectories().ToList().OrderByDescending( x => x.CreationTime).Select(x => x.Name).ToList();

        return stringList.ToArray();
    }

    /// <summary>
    /// Get array of save folder names in SaveData path in campaign folder
    /// </summary>
    /// <param name="campaign">Name of folder e.g. "MyCampaign"</param>
    /// <returns>string[] of File names in campaign folder</returns>
    public string[] GetSavedFilesFromCampaign(string campaign)
    {
        string folderPath = SerializationManager.CreatePath(campaign + "/");
        DirectoryInfo d = new DirectoryInfo(folderPath);
        //Sort files by date created and then convert to a list of strings
        var stringList = d.GetFiles().ToList().OrderByDescending(x => x.CreationTime).Select(x => x.Name).ToList();

        return stringList.ToArray();
    }

    /// <summary>
    /// Get array of template file names in Resource folder
    /// </summary>
    /// <returns>string[] of template File names in template folder</returns>
    public string[] GetTemplateNames()
    {
        TextAsset[] templateAssets = Resources.LoadAll<TextAsset>("Templates");

        var stringList = templateAssets.ToList().ConvertAll(x => x.name);

        return stringList.ToArray();
    }

    //Create a blank new savedata
    public void CreateCampaign() {

        const string newName = "NewCampaign";
       
        //Create hashset of campaign names to check against in order to make incremental campaign names
        HashSet<string> names = new HashSet<string>(GetSavedCampaigns());

        int count = 0;
        //Increment number as long as folder of that name and number already exist
        while (names.Contains(newName + (count > 0 ? count.ToString() : "")))
        {
            count++;
        }

        string folderPath = SerializationManager.CreatePath(newName + (count > 0 ? count.ToString(): ""));

        if (SerializationManager.CreateFolder(folderPath))
        {
            //Reload Campaigns
            ReloadCampaignTabs();
        } 
    }

    public void DeleteCampaignTabs()
    {
        Campaign[] tabs = GetComponentsInChildren<Campaign>();
        for (int i = 0; i < tabs.Count(); i++)
        {
            Destroy(tabs[i].gameObject);
        }
    
    }

    public void ReloadCampaignTabs()
    {
        DeleteCampaignTabs();

        foreach (string name in GetSavedCampaigns())
        {
            GameObject nc = Instantiate(campaignPrefab, transform);
            nc.transform.SetAsFirstSibling();
            nc.GetComponent<Campaign>().SetCampaignName(name);
        }

    }


    // Should create a save data for each folder in savedata
    void Start()
    {
        ReloadCampaignTabs();
    }

    void Awake()
    {
        instance = this;
    }
}