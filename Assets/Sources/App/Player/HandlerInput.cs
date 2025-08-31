using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private SendMessage sendMessage;
    [SerializeField] private SpawnBox spawnBox;
    [SerializeField] private SyncedAnimatorController animatorController;

    public void KeyboardInput()
    {
        var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movement.CmdMove(direction);
        animatorController.UpdateLocalAnimator(direction);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.CmdJump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            spawnBox.CmdSpawn();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            sendMessage.CmdSend();
        }
    }

}
