using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    private CharacterController characterController;
    private NetworkTransformUnreliable networkTransform;
    private NetworkIdentity networkIdentity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        networkTransform = GetComponent<NetworkTransformUnreliable>();
        networkIdentity = GetComponent<NetworkIdentity>();

        if (networkTransform == null)
        {
            Debug.LogError("NetworkTransform component missing!");
            return;
        }

        if (networkIdentity == null)
        {
            Debug.LogError("NetworkIdentity component missing!");
            return;
        }

        // Настраиваем ненадежный транспорт
        ConfigureUnreliableTransform();

        if (isLocalPlayer)
        {
            SetupCamera();
        }
    }

    private void ConfigureUnreliableTransform()
    {
        // ОСНОВНЫЕ НАСТРОЙКИ СИНХРОНИЗАЦИИ
        networkTransform.syncPosition = true;
        networkTransform.syncRotation = true;
        networkTransform.syncScale = false;

        // НАСТРОЙКИ ИНТЕРПОЛЯЦИИ
        networkTransform.interpolatePosition = true;
        networkTransform.interpolateRotation = true;

        // ЧАСТОТА СИНХРОНИЗАЦИИ (чаще для ненадежного транспорта)
        networkTransform.syncInterval = 0.05f;       // 20 раз в секунду
        
        // НАСТРОЙКИ ДЛЯ НЕНАДЕЖНОГО ТРАНСПОРТА
        networkTransform.compressRotation = false;    // Не сжимать для скорости

        Debug.Log("NetworkTransform configured for UNRELIABLE transport");
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        HandleMovement();
        HandleRotation();

        // Частая синхронизация для ненадежного транспорта
        if (Time.frameCount % 15 == 0) // Каждые 15 кадров
        {
            networkTransform.SetDirty();
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        Vector3 move = transform.TransformDirection(movement) * moveSpeed * Time.deltaTime;

        characterController.Move(move);

        // Гравитация
        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void SetupCamera()
    {
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

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }
}