using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    [Header("Network Settings")]
    [SyncVar] private Vector3 syncedPosition;
    [SyncVar] private Quaternion syncedRotation;
    [SyncVar] private Vector3 syncedVelocity;
    [SyncVar] private bool isGrounded;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalVelocity = 0f;

    // Интерполяция для плавного движения
    [SerializeField] private float positionLerpSpeed = 15f;
    [SerializeField] private float rotationLerpSpeed = 15f;

    // Тайминги для синхронизации
    private float lastSyncTime = 0f;
    private const float syncInterval = 0.1f; // 10 раз в секунду

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        SetupLocalCamera();
    }
        private void SetupLocalCamera()
    {
        if (!isLocalPlayer) return;
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            GameObject cameraContainer = new GameObject("CameraContainer");
    cameraContainer.transform.SetParent(transform);
            cameraContainer.transform.localPosition = new Vector3(0, 1.7f, 0);

    mainCamera.transform.SetParent(cameraContainer.transform);
            mainCamera.transform.localPosition = new Vector3(0, 0, -3f);
    mainCamera.transform.localRotation = Quaternion.identity;
        }
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            HandleLocalMovement();
            CheckForSync();
        }
        else
        {
            InterpolateMovement();
        }
    }

    private void HandleLocalMovement()
    {
        // Проверка земли
        isGrounded = characterController.isGrounded;

        // Сброс вертикальной скорости при касании земли
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        // Получение ввода
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Расчет движения
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        moveDirection = move * moveSpeed;

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = jumpForce;
        }

        // Гравитация
        verticalVelocity -= gravity * Time.deltaTime;
        moveDirection.y = verticalVelocity;

        // Применение движения
        characterController.Move(moveDirection * Time.deltaTime);

        // Обновление синхронизированных переменных
        syncedPosition = transform.position;
        syncedRotation = transform.rotation;
        syncedVelocity = moveDirection;
    }

    private void CheckForSync()
    {
        if (Time.time - lastSyncTime > syncInterval)
        {
            CmdUpdateMovement(transform.position, transform.rotation, moveDirection, isGrounded);
            lastSyncTime = Time.time;
        }
    }

    [Command]
    private void CmdUpdateMovement(Vector3 position, Quaternion rotation, Vector3 velocity, bool grounded)
    {
        syncedPosition = position;
        syncedRotation = rotation;
        syncedVelocity = velocity;
        isGrounded = grounded;

        // Распространяем изменения на всех клиентов
        RpcUpdateMovement(position, rotation, velocity, grounded);
    }

    [ClientRpc]
    private void RpcUpdateMovement(Vector3 position, Quaternion rotation, Vector3 velocity, bool grounded)
    {
        if (isLocalPlayer) return; // Не применяем к локальному игроку

        syncedPosition = position;
        syncedRotation = rotation;
        syncedVelocity = velocity;
        isGrounded = grounded;
    }

    private void InterpolateMovement()
    {
        // Интерполяция позиции
        transform.position = Vector3.Lerp(transform.position, syncedPosition, positionLerpSpeed * Time.deltaTime);

        // Интерполяция вращения
        transform.rotation = Quaternion.Lerp(transform.rotation, syncedRotation, rotationLerpSpeed * Time.deltaTime);

        // Для не-локальных игроков также применяем физику
        if (!characterController.isGrounded)
        {
            characterController.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
        }
    }

    // Метод для принудительной синхронизации
    [ClientCallback]
    public void ForceSync()
    {
        if (isLocalPlayer)
        {
            CmdUpdateMovement(transform.position, transform.rotation, moveDirection, isGrounded);
        }
    }

    // Вызывается при телепортации для мгновенной синхронизации
    [Client]
    public void Teleport(Vector3 newPosition)
    {
        if (isLocalPlayer)
        {
            transform.position = newPosition;
            CmdUpdateMovement(newPosition, transform.rotation, Vector3.zero, true);
        }
    }

}