using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class NetworkHandler : NetworkBehaviour {

    SyncListPagePacket pageSyncList = new SyncListPagePacket();
    Dictionary<string, PageData> pages = new Dictionary<string, PageData>();


    public override void OnStartClient()
    {
        base.OnStartClient();
        pageSyncList.Callback = OnListChange;
    }

    private void OnListChange(SyncListPagePacket.Operation op, int index)
    {
       
    }

    // Use this for initialization
    void Start () {
        
 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Command]
    public void CmdSendPagePacket(PagePacket data)
    {
        //Add it to list
    }

    public void UpdatePagePacket()
    {

    }

}


public struct PagePacket
{
    public enum PageType { SharedImage, StatBlockUI, Map }

    public string name;
    public PageType pageType;
    public byte[] data;
}

public class SyncListPagePacket : SyncListStruct<PagePacket>{}


[Serializable]
public abstract class PageData
{
    //On/before/after serialize/deserialize
}

[Serializable]
public class SharedImageData : PageData
{
    
    //ImageConversion.LoadImage(Texture2D, data);
}

[Serializable]
public class StatBlockUIData : PageData
{
    byte[] textBytes;

    //TextAsset t = new TextAsset();
    //t.bytes
}

[Serializable]
public class MapData : PageData
{
    
}
