using Mirror;
using System.Collections;
using UnityEngine;


public class UpdateStateManager : NetworkBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private HandlerInput handlerInput;
    void Update()
    {
        if (!isLocalPlayer) return;

        HandleInput();
        HandleGravity();
    }

    private void HandleInput()
    {
        handlerInput.KeyboardInput();
    }

    private void HandleGravity()
    {
        playerMovement.HandleGravity();
    }


}
