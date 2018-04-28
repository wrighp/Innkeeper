using UnityEngine;
using UnityEngine.Networking;

public class ClientController : NetworkBehaviour {

    /// <summary>
    /// Called whenever a client joins the game
    /// Create a statblock to add to the scene
    /// </summary>
    override public void OnStartClient()
    {
        base.OnStartClient();
        if(isServer)
        {
            PagePacket packet = new PagePacket();
            packet.name = "statblock";
            packet.pageType = PagePacket.PageType.StatBlockUI;
            StatBlockUIData sbd = new StatBlockUIData();
            sbd.name = packet.name;
            packet.data = SerializationManager.SerializeObject(sbd);
            packet.destroy = false;
            CmdSendPagePacket(packet);
        }
    }

    /// <summary>
    /// Player object has authority to call commands to the server
    /// Send packet data
    /// </summary>
    /// <param name="packet">Binary data for the current page to be sent to clients</param>
    [Command]
    public void CmdSendPagePacket(PagePacket packet)
    {
        NetworkHandler.instance.CmdSendPagePacket(packet);
    }

    /// <summary>
    /// Player object has authority to call commands to the server
    /// Delete a page from the campaign
    /// </summary>
    /// <param name="name">Name of thepage to be deleted</param>
    [Command]
    public void CmdDestroyPage(string name)
    {
        NetworkHandler.instance.CmdDestroyPage(name);
    }
}
