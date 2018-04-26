﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Campaign and Campaign file should prbably inherit from an abstract class
public class CampaignFile : MonoBehaviour {

    string fileName;
    string extension;
    Campaign campaign;

    public void SetCampaignName(Campaign c) {
        campaign = c;
    }

    //Set name of folder on instantiation
    public void SetFileName(string cname) {
        fileName = cname.Split('.')[0];
        extension = cname.Split('.')[1];
        GetComponentInChildren<InputField>().text = fileName;
    }

    public string GetFileName() {
        return fileName;
    }
    public string GetExtension()
    {
        return extension;
    }

    public Campaign GetCampaign()
    {
        return campaign;
    }
    public void LoadFile()
    {
        PageManager.instance.SwitchPage(this);
    }

    //Change the name pf the folder if and when the user changes the name
    public void ChangeFileName() {
        string cname = GetComponentInChildren<InputField>().text;
        var oldPath = SerializationManager.CreatePath(campaign.GetCampaignName() + "/" + fileName + "." + extension);
        var newPath = SerializationManager.CreatePath(campaign.GetCampaignName() + "/" + cname + "." + extension);
        if (oldPath.Equals(newPath) || Directory.Exists(newPath)) {
            campaign.LoadFiles();
            return;
        }

        Debug.LogFormat("File: {0}\nRenaming to: {1}", oldPath, newPath);
        try {
            Directory.Move(oldPath, newPath);
            fileName = cname;
            campaign.LoadFiles();
        } catch (IOException ex) {
            Debug.LogException(ex);
        }
    }

    //Delete file
    public void DeleteFile() {
        SerializationManager.DeleteFile(SerializationManager.CreatePath(campaign.GetCampaignName() + "/" + fileName + "." + extension));
        campaign.LoadFiles();
    }


}
