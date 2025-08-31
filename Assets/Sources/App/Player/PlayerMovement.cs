using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private SyncedAnimatorController syncedAnimatorController;
    [Header("Parameters")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Vector3 jumpVelocity;


    [Command]
    public void CmdSyncMovement(Vector3 position, Quaternion rotation)
    {
        // ������ ��������� ���� �������
        transform.position = position;
        transform.rotation = rotation;
        if (!isLocalPlayer) return;
        // �������������� � ������� ���������
        RpcSyncMovement(position, rotation);
    }

    [ClientRpc]
    public void RpcSyncMovement(Vector3 position, Quaternion rotation)
    {
        // �� ��������� ���������� ������ (�� ��� � ���������� �������)
        if (isLocalPlayer) return;

        transform.position = position;
        transform.rotation = rotation;
    }


    public void CmdMove(Vector2 moveDirection)
    {
        if (!characterController.isGrounded) {
            HandleMovement(moveDirection);
        }
    }
    private void HandleMovement(Vector2 moveDirection)
    {
        Vector3 move = new Vector3(moveDirection.x, 0, moveDirection.y);
        move = transform.TransformDirection(move);
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }
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
        if (mainCamera != null)
        {
            // ������� ��������� ��� ������
            GameObject cameraContainer = new GameObject("CameraContainer");
            cameraContainer.transform.SetParent(transform);
            cameraContainer.transform.localPosition = new Vector3(0, 1.7f, 0);

            // ����������� ������
            mainCamera.transform.SetParent(cameraContainer.transform);
            mainCamera.transform.localPosition = new Vector3(0, 0, -3f);
            mainCamera.transform.localRotation = Quaternion.identity;
        }
    }

    public void HandleGravity()
    {
        if (characterController.isGrounded && jumpVelocity.y < 0)
        {
            jumpVelocity.y = -2f;
            if (syncedAnimatorController.GetJump())
            {
                CmdLand();
            }
        }

        jumpVelocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(jumpVelocity * Time.deltaTime);
    }

    [Command]
    private void CmdLand()
    {
        syncedAnimatorController.UpdateSyncJump(false);
    }

    [Command]
    public void CmdJump()
    {
        // ��������� �������� ������ �� �������
        jumpVelocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        syncedAnimatorController.UpdateSyncJump(true);
    }


}
