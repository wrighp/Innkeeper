using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using UnityEngine;

public class NetworkHandler : NetworkBehaviour
{

    SyncListPagePacket pageSyncList = new SyncListPagePacket();
    Dictionary<string, GameObject> pages = new Dictionary<string, GameObject>();


    public override void OnStartClient()
    {
        base.OnStartClient();
        pageSyncList.Callback = OnListChange;
        //OnListChange not called on initialization, so rebuild all pages based on synclist
        RebuildAllPages();
    }

    //Client call
    private void OnListChange(SyncListPagePacket.Operation op, int index)
    {
        //Go through entire list and rebuild dictionary for now, every time OnListChange happens
        //Individual Pages should ideally be destroyed and created with proper checking of dictionary O(1) not O(n)
        RebuildAllPages();
    }
    
    /// <summary>
    /// Destroy all current pages and recreate all pages from synclist
    /// </summary>
    private void RebuildAllPages()
    {
        ClearPages();
        for(int i = 0; i < pageSyncList.Count; i++)
        {
            BuildPage(pageSyncList[i]);
        }
    }

    /// <summary>
    /// Destroy all gameobject pages and clear the dictionary
    /// This does not clear the synced list of data
    /// </summary>
    private void ClearPages()
    {
        foreach(GameObject page in pages.Values)
        {
            DestroyPage(page);
        }
        pages.Clear();
    }

    private void BuildPage(PagePacket packet)
    {
        switch (packet.pageType)
        {
            case PagePacket.PageType.StatBlockUI:
                {
                    StatBlockUIData uiData = (StatBlockUIData)SerializationManager.LoadObject(packet.data);
                    //Create statblockui
                    break;
                }
            default:
                {
                    Debug.LogError("Received Unsupported PagePacket type: " + packet.pageType);
                    break;
                }
        }
    }

    private void DestroyPage(GameObject page)
    {

    }

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Function called on server to destroy a page of a given name
    /// </summary>
    /// <param name="name"></param>
    public void CmdDestroyPage(string name)
    {
        var packet = new PagePacket();
        packet.destroy = true;
        packet.name = name;
        CmdSendPagePacket(packet);
    }

    /// <summary>
    /// Function called on server when PagePacket data is sent to it from a player client
    /// Can be used to create new pages, destroy old ones, and overrite existing pages
    /// </summary>
    /// <param name="packet"></param>
    public void CmdSendPagePacket(PagePacket packet)
    {

        //Override value in list if page name already exists
        string name = packet.name;
        for(int i = 0; i < pageSyncList.Count; i++)
        {
            if (pageSyncList[i].name.Equals(name))
            {
                if (packet.destroy)
                {
                    pageSyncList.RemoveAt(i);
                }
                else
                {
                    pageSyncList[i] = packet;
                }
                return;
            }
        }

        //If not found in list and wasn't destroyed add to list
        if (!packet.destroy)
        {
            pageSyncList.Add(packet);
        }
        else
        {
            //Page set to destroy could not actually be found
        }
        return;
    }





}

/// <summary>
/// Page packet sent to server which then modifies list of pagepackets which are synced to clients
/// </summary>
public struct PagePacket
{
    public enum PageType { TestData, SharedImage, StatBlockUI, Map }

    public string name;
    public PageType pageType;
    public byte[] data; //Serialized data of page
    public bool destroy; //Signals page with this name to be destroyed
}

public class SyncListPagePacket : SyncListStruct<PagePacket> { }


[Serializable]
public abstract class PageData
{
    //On/before/after serialize/deserialize
    //public abstract void OnAfterDeserialize();
    //public abstract void OnBeforeSerialize();
}

[Serializable]
public class SharedImageData : PageData
{

    //ImageConversion.LoadImage(Texture2D, data);
}

[Serializable]
public class StatBlockUIData : PageData
{
   public string text;
}

[Serializable]
public class MapData : PageData
{

}
