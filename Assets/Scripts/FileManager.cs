using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private ScrollRect rect;

    //user chooses standard template to be saved as a modified template
    void saveTemplate(string template)
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

    void pullAssetNames()
    {
        string assetPath = SerializationManager.CreatePath("");
        DirectoryInfo d = new DirectoryInfo(assetPath);
        Dictionary<string, string> assetFiles = new Dictionary<string, string>();
        foreach (var file in d.GetFiles())
        {
            assetFiles.Add(file.Name, file.FullName);
        }

        //Here, the user selects the name of the file they want displayed on the page
        string chosenFile;
        SerializationManager.LoadObject(assetFiles[chosenFile]);
    }

    // Use this for initialization
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
