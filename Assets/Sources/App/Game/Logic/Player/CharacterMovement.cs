using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] 
    private CharacterController controller;
    [SerializeField]
    private NetworkTransformReliable networkTransform;
   [SerializeField]
    private float velocity;

    private void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

   // [Command]
    private void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(direction * Time.deltaTime * velocity);
    }
}
