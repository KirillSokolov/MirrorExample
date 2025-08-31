using Mirror;
using System.Collections;


public class UpdateStateManager : NetworkBehaviour
{
    private PlayerMovement playerMovement;
    private HandlerInput handlerInput;

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
