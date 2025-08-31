using Mirror;
using UnityEngine;

public class SyncedAnimatorController : NetworkBehaviour
{
    [Header("Animator References")]
    [SerializeField] private Animator animator;
    [SerializeField] private NetworkAnimator networkAnimator;

    [Header("Animation Parameters")]
    [SerializeField] private string moveXParam = "MoveX";
    [SerializeField] private string moveYParam = "MoveY";
    [SerializeField] private string isMovingParam = "IsMoving";
    [SerializeField] private string isGroundedParam = "IsGrounded";
    [SerializeField] private string jumpParam = "Jump";
    [SerializeField] private string attackParam = "Attack";

    [SyncVar]
    private float syncMoveX;

    [SyncVar]
    private float syncMoveY;

    [SyncVar]
    private bool syncIsMoving;

    [SyncVar(hook = nameof(OnJumpingChanged))]
    private bool isJumping;

    private CharacterController characterController;
    private Vector2 movementInput;
    private bool isGrounded;
 
    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (networkAnimator == null)
            networkAnimator = GetComponent<NetworkAnimator>();

        characterController = GetComponent<CharacterController>();
    }
 
  
    [Command]
    public void UpdateLocalAnimator(Vector2 direction)
    {
        if (animator == null) return;
        animator.SetFloat(moveXParam, direction.x);
        animator.SetFloat(moveYParam, direction.y);
        animator.SetBool(isMovingParam, direction.magnitude > 0.1f);
        animator.SetBool(isGroundedParam, isJumping);
        UpdateSyncParameters();
    }
    public bool GetJump()
    {
        return isJumping;
    }
    [Server]
    public void UpdateSyncJump(bool isJumping)
    {
        this.isJumping = isJumping;
    }
    
        [Server]
    private void UpdateSyncParameters()
    {
        // Сервер обновляет SyncVar параметры
        syncMoveX = movementInput.x;
        syncMoveY = movementInput.y;
        syncIsMoving = movementInput.magnitude > 0.1f;
    }
    private void OnJumpingChanged(bool oldValue, bool newValue)
    {
        // Дополнительная логика при изменении состояния прыжка
    }
}