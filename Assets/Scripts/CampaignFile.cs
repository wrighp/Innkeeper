using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Campaign and Campaign file should prbably inherit from an abstract class
public class CampaignFile : MonoBehaviour {

    string fileName;
    string extension;
    Campaign campaign;

    /// <summary>
    /// Set the related campaign name for the current file
    /// </summary>
    /// <param name="c">Name of the campaign</param>
    public void SetCampaignName(Campaign c)
    {
        campaign = c;
    }

    /// <summary>
    /// Get the extension type and name of the file
    /// </summary>
    /// <param name="cname"></param>
    public void SetFileName(string cname)
    {
        fileName = cname.Split('.')[0];
        extension = cname.Split('.')[1];
        GetComponentInChildren<InputField>().text = fileName;
    }

    /// <summary>
    /// Return file name
    /// </summary>
    /// <returns>nameof the file without the extension</returns>
    public string GetFileName()
    {
        return fileName;
    }

    /// <summary>
    /// Return the extension
    /// </summary>
    /// <returns>Extension tyoe as string</returns>
    public string GetExtension()
    {
        return extension;
    }

    /// <summary>
    /// Return the campaign name
    /// </summary>
    /// <returns>Campaign name</returns>
    public Campaign GetCampaign()
    {
        return campaign;
    }

    /// <summary>
    /// Event Listener, Load the data for the current page type into a game object for viewing 
    /// </summary>
    public void LoadFile()
    {
        PageManager.instance.SwitchPage(this);
    }

    /// <summary>
    /// Event Listener, change the file name based on the changes the user made into the input field
    /// </summary>
    public void ChangeFileName()
    {
        string cname = GetComponentInChildren<InputField>().text;
        var oldPath = SerializationManager.CreatePath(campaign.GetCampaignName() + "/" + fileName + "." + extension);
        var newPath = SerializationManager.CreatePath(campaign.GetCampaignName() + "/" + cname + "." + extension);
        if (oldPath.Equals(newPath) || Directory.Exists(newPath))
        {
            campaign.LoadFiles();
            return;
        }

        Debug.LogFormat("File: {0}\nRenaming to: {1}", oldPath, newPath);
        try
        {
            Directory.Move(oldPath, newPath);
            fileName = cname;
            campaign.LoadFiles();
        }
        catch (IOException ex)
        {
            Debug.LogException(ex);
        }
    }

    /// <summary>
    /// Delete the file from the campaign folder
    /// </summary>
    public void DeleteFile()
    {
        SerializationManager.DeleteFile(SerializationManager.CreatePath(campaign.GetCampaignName() + "/" + fileName + "." + extension));
        campaign.LoadFiles();
    }
}
