using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
  
    public void SendGreeting(string nickName)
    {
        var text = $"Привет от {nickName}";
        RpcReceiveChatMessage(text);
    }

   // [ClientRpc]
    private void RpcReceiveChatMessage(string messageText)
    {
        Debug.Log(messageText);
    }
}
