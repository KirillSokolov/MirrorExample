using Mirror;
using UnityEngine;

public class SendMessage : NetworkBehaviour
{
    private ISaveLoadUserData saveLoadUserData;

    private void Awake()
    {
        saveLoadUserData = SaveLoadDataImpl.Instance;
    }

    [Command]
    public void Send()
    {
        RpcReceiveChatMessage(saveLoadUserData.GetNickname());
    }

    [ClientRpc]
    private void RpcReceiveChatMessage(string nickname)
    {
        Debug.Log($"Привет от {nickname}");
    }
}
