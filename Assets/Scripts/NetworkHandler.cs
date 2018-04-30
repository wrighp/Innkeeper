using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NetworkHandler : NetworkBehaviour
{
    public static NetworkHandler instance;
    SyncListPagePacket pageSyncList = new SyncListPagePacket();
    public Dictionary<string, GameObject> pages = new Dictionary<string, GameObject>();
    public GameObject[] prefabs;

    /// <summary>
    /// Called on creation
    /// </summary>
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    /// <summary>
    /// Called when a client joins the game
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();
        //Add an event listener to allow the suer to get updates to pages when someone makes a change
        pageSyncList.Callback = OnListChange;
        //OnListChange not called on initialization, so rebuild all pages based on synclist
        RebuildAllPages();
    }

    /// <summary>
    /// Rebuild all pages when a page is detected as modified
    /// </summary>
    /// <param name="op">Operation performed on the synclist</param>
    /// <param name="index">Index of the page tha twas modifed in the list</param>
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
        print("Sycnlist: " + pageSyncList.Count);
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
            Destroy(page);
        }
        pages.Clear();
    }

    /// <summary>
    /// Build a page based on recived page packets
    /// </summary>
    /// <param name="packet">Contains all data needed to build a page</param>
    private void BuildPage(PagePacket packet)
    {
        switch (packet.pageType)
        {
            case PagePacket.PageType.StatBlockUI:
                {
                    StatBlockUIData uiData = (StatBlockUIData)SerializationManager.LoadObject(packet.data);
                    //Create statblockui
                    GameObject parent = GameObject.Find("Canvas/Page View/Viewport");
                    GameObject statBlock = Instantiate(prefabs[0], parent.transform);
                    statBlock.GetComponent<StatBlockForm>().BuildPage(uiData);
                    pages.Add(packet.name, statBlock);
                    break;
                }
            default:
                {
                    Debug.LogError("Received Unsupported PagePacket type: " + packet.pageType);
                    break;
                }
        }
    }

    /// <summary>
    /// Destory a page
    /// </summary>
    /// <param name="page">Page to be destroyed</param>
    private void DestroyPage(GameObject page)
    {

    }

    /// <summary>
    /// Function called on server to destroy a page of a given name
    /// </summary>
    /// <param name="name">Name pf the page to be deleted</param>
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
    /// <param name="packet">Packet sent to build pages on clients</param>
    public void CmdSendPagePacket(PagePacket packet)
    {
        //Override value in list if page name already exists
        //print("PacketNameAfterSend: " + packet.name);
        for (int i = 0; i < pageSyncList.Count; i++)
        {
            if (pageSyncList[i].name.Equals(packet.name))
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
            //print("Added Packet: " + packet.name);
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
