using Mirror;
using System.Collections;
using UnityEngine;


public class UpdateStateManager : NetworkBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private HandlerInput handlerInput;
    void Update()
    {
        if (!isLocalPlayer)
        {
            playerMovement.HandleRemoteMovement();
            return;
        }

        HandleInput();
        playerMovement.SyncWithServer();
    }

    private void HandleInput()
    {
        handlerInput.KeyboardInput();
    }

}
