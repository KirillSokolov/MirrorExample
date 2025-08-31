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

    [SyncVar(hook = nameof(OnMoveXChanged))]
    private float syncMoveX;

    [SyncVar(hook = nameof(OnMoveYChanged))]
    private float syncMoveY;

    [SyncVar(hook = nameof(OnIsMovingChanged))]
    private bool syncIsMoving;

    [SyncVar(hook = nameof(OnIsGroundedChanged))]
    private bool syncIsGrounded;

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

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("🖥️ Server animator controller initialized");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
        {
            Debug.Log("🎮 Local player animator controller initialized");
        }
        else
        {
            Debug.Log("👥 Remote player animator controller initialized");
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            HandleLocalInput();
            UpdateLocalAnimator();
        }
        else
        {
            // Для удаленных игроков анимации обновляются через SyncVar хуки
        }

        // Сервер обновляет синхронизированные параметры
        if (isServer)
        {
            UpdateSyncParameters();
        }
    }

    private void HandleLocalInput()
    {
        // Получаем ввод для анимаций
        movementInput = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        // Отправляем на сервер для синхронизации
        if (movementInput != Vector2.zero)
        {
            CmdUpdateMovement(movementInput);
        }

        // Проверка земли
        isGrounded = characterController != null && characterController.isGrounded;

        // Обработка прыжка
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            CmdTriggerJump();
        }

        // Обработка атаки
        if (Input.GetMouseButtonDown(0))
        {
            CmdTriggerAttack();
        }
    }

    private void UpdateLocalAnimator()
    {
        if (animator == null) return;

        // Локальное обновление аниматора для плавности
        animator.SetFloat(moveXParam, movementInput.x);
        animator.SetFloat(moveYParam, movementInput.y);
        animator.SetBool(isMovingParam, movementInput.magnitude > 0.1f);
        animator.SetBool(isGroundedParam, isGrounded);
    }

    [Server]
    private void UpdateSyncParameters()
    {
        // Сервер обновляет SyncVar параметры
        syncMoveX = movementInput.x;
        syncMoveY = movementInput.y;
        syncIsMoving = movementInput.magnitude > 0.1f;
        syncIsGrounded = isGrounded;
    }

    #region Network Commands

    [Command]
    private void CmdUpdateMovement(Vector2 moveInput)
    {
        movementInput = moveInput;
    }

    [Command]
    private void CmdTriggerJump()
    {
        // Триггерим прыжок на сервере
        syncIsGrounded = false;
        RpcPlayJumpAnimation();
    }

    [Command]
    private void CmdTriggerAttack()
    {
        // Триггерим атаку на сервере
        RpcPlayAttackAnimation();
    }

    #endregion

    #region Client RPCs

    [ClientRpc]
    private void RpcPlayJumpAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(jumpParam);
        }
    }

    [ClientRpc]
    private void RpcPlayAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(attackParam);
        }
    }

    #endregion

    #region SyncVar Hooks

    private void OnMoveXChanged(float oldValue, float newValue)
    {
        if (!isLocalPlayer && animator != null)
        {
            animator.SetFloat(moveXParam, newValue);
        }
    }

    private void OnMoveYChanged(float oldValue, float newValue)
    {
        if (!isLocalPlayer && animator != null)
        {
            animator.SetFloat(moveYParam, newValue);
        }
    }

    private void OnIsMovingChanged(bool oldValue, bool newValue)
    {
        if (!isLocalPlayer && animator != null)
        {
            animator.SetBool(isMovingParam, newValue);
        }
    }

    private void OnIsGroundedChanged(bool oldValue, bool newValue)
    {
        if (!isLocalPlayer && animator != null)
        {
            animator.SetBool(isGroundedParam, newValue);
        }
    }

    #endregion

    #region Public Methods

    public void SetMovement(float x, float y)
    {
        if (isLocalPlayer)
        {
            CmdUpdateMovement(new Vector2(x, y));
        }
    }

    public void TriggerJump()
    {
        if (isLocalPlayer)
        {
            CmdTriggerJump();
        }
    }

    public void TriggerAttack()
    {
        if (isLocalPlayer)
        {
            CmdTriggerAttack();
        }
    }

    public void SetBool(string paramName, bool value)
    {
        if (isLocalPlayer)
        {
            CmdSetBool(paramName, value);
        }
    }

    public void SetFloat(string paramName, float value)
    {
        if (isLocalPlayer)
        {
            CmdSetFloat(paramName, value);
        }
    }

    public void SetTrigger(string paramName)
    {
        if (isLocalPlayer)
        {
            CmdSetTrigger(paramName);
        }
    }

    [Command]
    private void CmdSetBool(string paramName, bool value)
    {
        RpcSetBool(paramName, value);
    }

    [Command]
    private void CmdSetFloat(string paramName, float value)
    {
        RpcSetFloat(paramName, value);
    }

    [Command]
    private void CmdSetTrigger(string paramName)
    {
        RpcSetTrigger(paramName);
    }

    [ClientRpc]
    private void RpcSetBool(string paramName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(paramName, value);
        }
    }

    [ClientRpc]
    private void RpcSetFloat(string paramName, float value)
    {
        if (animator != null)
        {
            animator.SetFloat(paramName, value);
        }
    }

    [ClientRpc]
    private void RpcSetTrigger(string paramName)
    {
        if (animator != null)
        {
            animator.SetTrigger(paramName);
        }
    }

    #endregion

    [ContextMenu("Test Jump Animation")]
    public void TestJump()
    {
        TriggerJump();
    }

    [ContextMenu("Test Attack Animation")]
    public void TestAttack()
    {
        TriggerAttack();
    }

    [ContextMenu("Print Animator Info")]
    public void PrintAnimatorInfo()
    {
        if (animator != null)
        {
            Debug.Log($"🔍 Animator Info:");
            Debug.Log($"- Parameters count: {animator.parameterCount}");
            Debug.Log($"- Is Initialized: {animator.isInitialized}");
            Debug.Log($"- Is Valid: {animator.isActiveAndEnabled}");

            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                Debug.Log($"- {param.name} ({param.type})");
            }
        }
    }
}