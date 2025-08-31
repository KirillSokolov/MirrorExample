using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private SyncedAnimatorController animatorController;
   
    [Header("Parameters")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Vector3 jumpVelocity;

    public void Move(Vector2 moveDirection)
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
        UpdateAnimator(moveDirection);
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
            // Создаем контейнер для камеры
            GameObject cameraContainer = new GameObject("CameraContainer");
            cameraContainer.transform.SetParent(transform);
            cameraContainer.transform.localPosition = new Vector3(0, 1.7f, 0);

            // Настраиваем камеру
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
        }

        jumpVelocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(jumpVelocity * Time.deltaTime);
    }

    private void UpdateAnimator(Vector2 moveDirection)
    {
        if (animatorController != null)
        {
            // Локальное обновление анимаций
            animatorController.SetMovement(moveDirection.x, moveDirection.y);

            // Можно также использовать прямые вызовы
            animatorController.SetBool("IsGrounded", characterController.isGrounded);
            animatorController.SetBool("IsMoving", moveDirection.magnitude > 0.1f);
        }
    }

    public void Jump()
    {
        jumpVelocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);

        if (animatorController != null)
        {
            animatorController.TriggerJump();
        }
    }

    [Command]
    private void CmdUpdateAnimatorParameters()
    {
        // Серверная синхронизация параметров
        if (animatorController != null)
        {
            // Параметры синхронизируются через SyncVar в SyncedAnimatorController
        }
    }

    [ContextMenu("Test Animation Sync")]
    public void TestAnimationSync()
    {
        if (animatorController != null)
        {
            animatorController.TriggerAttack();
        }
    }
}
