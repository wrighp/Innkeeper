using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class FileManager : MonoBehaviour
{
    public GameObject campaignPrefab;

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
        
        var stringList = d.GetDirectories().ToList().ConvertAll(x => x.Name );
        
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

        var stringList = d.GetFiles().ToList().ConvertAll(x => x.Name);

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
        GameObject nc = Instantiate(campaignPrefab, transform);
        nc.transform.SetAsFirstSibling();
        nc.GetComponent<Campaign>().campaignName = "NewCampaign";
    }

    // Should create a save data for each folder in savedata
    void Start()
    {
        
    }
}