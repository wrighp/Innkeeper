using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientController : NetworkBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
    }

    override public void OnStartClient()
    {
        base.OnStartClient();
        if(isServer)
        {
            print("remaking cascas");
            PagePacket packet = new PagePacket();
            packet.name = "test";
            packet.pageType = PagePacket.PageType.StatBlockUI;
            StatBlockUIData sbd = new StatBlockUIData();
            sbd.name = packet.name;
            packet.data = SerializationManager.SerializeObject(sbd);
            packet.destroy = false;
            CmdSendPagePacket(packet);
        }
    }

    [Command]
    public void CmdSendPagePacket(PagePacket packet)
    {
        //print("PacketNameBeforeSend: " + packet.name);
        NetworkHandler.instance.CmdSendPagePacket(packet);
    }
    [Command]
    public void CmdDestroyPage(string name)
    {
        NetworkHandler.instance.CmdDestroyPage(name);
    }
}
