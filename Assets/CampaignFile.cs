using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Campaign and Campaign file should prbably inherit from an abstract class
public class CampaignFile : MonoBehaviour {

    string fileName;
    Campaign campaign;

    public void SetCampaignName(Campaign c) {
        campaign = c;
    }

    //Set name of folder on instantiation
    public void SetFileName(string cname) {
        fileName = name;
        GetComponentInChildren<InputField>().text = cname;
    }

    public string GetFileName() {
        return fileName;
    }


    //Change the name pf the folder if and when the user changes the name
    public void ChangeFileName(string cname) {
        var oldPath = SerializationManager.CreatePath(campaign.GetCampaignName() + "/" + fileName);
        var newPath = SerializationManager.CreatePath(campaign.GetCampaignName() + "/" + cname);
        if (oldPath.Equals(newPath) || Directory.Exists(newPath)) {
            //campaign.ReloadFiles(transform.parent.gameObject);
            return;
        }

        Debug.LogFormat("File: {0}\nRenaming to: {1}", oldPath, newPath);
        try {
            Directory.Move(oldPath, newPath);
            fileName = cname;
            //campaign.ReloadFiles(transform.parent.gameObject);
        } catch (IOException ex) {
            Debug.LogException(ex);
        }
    }

    //Delete file
    public void DeleteFile() {
        SerializationManager.DeleteFile(SerializationManager.CreatePath(campaign.GetCampaignName() + "/" + fileName));
        //campaign.ReloadFiles(transform.parent.gameObject);
    }


}
