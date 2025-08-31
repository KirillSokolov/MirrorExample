using Mirror;
using UnityEngine;

public class SendMessage : NetworkBehaviour
{
    [Command]
    public void CmdSend()
    {
        RpcReceiveChatMessage(gameObject.name);
    }

    [ClientRpc]
    private void RpcReceiveChatMessage(string nickname)
    {
        Debug.Log($"Привет от {nickname}");
    }

 
}
