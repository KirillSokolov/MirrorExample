using Mirror;
using System;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;

    [Header("Sync Vars")]
    [SyncVar(hook = nameof(OnPositionChanged))]
    private Vector3 syncPosition;

    [SyncVar(hook = nameof(OnRotationChanged))]
    private Quaternion syncRotation;

    [Header("Parameters")]
    [SerializeField] private float moveSpeed = 5f;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // InitializeLocalPlayer();
        SetupLocalCamera();
    }

    private void SetupLocalCamera()
    {
        if (!isLocalPlayer) return;

        Camera mainCamera = Camera.main;
        if (mainCamera != null) { 
            
                GameObject cameraContainer = new GameObject("CameraContainer");
                cameraContainer.transform.SetParent(transform);
                cameraContainer.transform.localPosition = new Vector3(0, 1.7f, 0);

                mainCamera.transform.SetParent(cameraContainer.transform);
                mainCamera.transform.localPosition = new Vector3(0, 0, -3f);
                mainCamera.transform.localRotation = Quaternion.identity;
            }
    }


    public void HandleLocalMovement(Vector2 direction)
    {
        Vector3 move = new Vector3(direction.x, 0, direction.y);
        move = transform.TransformDirection(move);
        characterController.Move(move * moveSpeed * Time.deltaTime);
        syncPosition = move;
    }

    public void SyncWithServer()
    {
        if (Vector3.Distance(transform.position, syncPosition) > 0.1f ||
            Quaternion.Angle(transform.rotation, syncRotation) > 1f)
        {
            CmdSyncTransform(transform.position, transform.rotation);
        }
    }

    [Command]
    private void CmdSyncTransform(Vector3 position, Quaternion rotation)
    {
        syncPosition = position;

        if (rotation != Quaternion.identity && rotation.eulerAngles.sqrMagnitude > 0.01f)
        {
            syncRotation = rotation;
        }
    }

    public void HandleRemoteMovement()
    {
        if (isLocalPlayer) return;
        try
        {
            if (Vector3.Distance(transform.position, syncPosition) > 0.001f)
            {
                transform.position = Vector3.Lerp(transform.position, syncPosition, Time.deltaTime * 10f);
            }

            if (syncRotation != Quaternion.identity &&
                Quaternion.Angle(transform.rotation, syncRotation) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, syncRotation, Time.deltaTime * 10f);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Remote movement error: {e.Message}");
        }
    }


    private void OnPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        syncPosition = newPos;
        HandleRemoteMovement();
    }

    private void OnRotationChanged(Quaternion oldRot, Quaternion newRot)
    {
        syncRotation = newRot;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        syncPosition = transform.position;
        syncRotation = transform.rotation;
    }
}