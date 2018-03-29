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

    [Command]
    public void CmdSendPagePacket(PagePacket packet)
    {
        NetworkHandler.instance.CmdSendPagePacket(packet);
    }
    [Command]
    public void CmdDestroyPage(string name)
    {
        NetworkHandler.instance.CmdDestroyPage(name);
    }
}
