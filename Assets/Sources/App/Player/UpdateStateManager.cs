using Mirror;
using System.Collections;
using UnityEngine;


public class UpdateStateManager : NetworkBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private HandlerInput handlerInput;
    void Update()
    {
        if (!isLocalPlayer)   return;
        
        HandleInput();
        playerMovement.CheckMoveChanges();
    }

    private void HandleInput()
    {
        handlerInput.KeyboardInput();
    }

}
