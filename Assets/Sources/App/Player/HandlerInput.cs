using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private SendMessage sendMessage;
    [SerializeField] private SpawnBox spawnBox;

    public void KeyboardInput()
    {
        movement.Move(new Vector2(Input.GetAxis("Horizontal"),  Input.GetAxis("Vertical")));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.Jump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            spawnBox.Spawn();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            sendMessage.Send();
        }
    }

}
