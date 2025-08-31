using Mirror;
using UnityEngine;

public class PlayerController: NetworkBehaviour
{
    [SerializeField] private TextMesh nicknameText;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private SyncedAnimatorController animatorController;

    private Vector2 movementInput;
    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        nicknameText.text = SaveLoadDataImpl.Instance.GetNickname();
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (animatorController == null)
            animatorController = GetComponent<SyncedAnimatorController>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        HandleInput();
        HandleMovement();
        HandleGravity();
        UpdateAnimator();
    }

    private void HandleInput()
    {
        movementInput = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void HandleMovement()
    {
        if (characterController == null) return;

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        move = transform.TransformDirection(move);
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (characterController == null) return;

        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        if (animatorController != null)
        {
            // Локальное обновление анимаций
            animatorController.SetMovement(movementInput.x, movementInput.y);

            // Можно также использовать прямые вызовы
            animatorController.SetBool("IsGrounded", isGrounded);
            animatorController.SetBool("IsMoving", movementInput.magnitude > 0.1f);
        }
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);

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