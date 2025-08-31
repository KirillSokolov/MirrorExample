using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    private CharacterController characterController;
    private Camera playerCamera;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (isLocalPlayer)
        {
            SetupCamera();
        }

        // Настраиваем NetworkTransform
        ConfigureNetworkTransform();
    }

    private void ConfigureNetworkTransform()
    {
        // В Mirror 96.0.1 используется NetworkTransform, а не NetworkTransformReliable
        var networkTransform = GetComponent<NetworkTransformReliable>();
        if (networkTransform != null)
        {
            // Настройки синхронизации
            networkTransform.syncPosition = true;
            networkTransform.syncRotation = true;
            networkTransform.syncScale = false;

            // Настройки интерполяции
            networkTransform.interpolatePosition = true;
            networkTransform.interpolateRotation = true;

            // Интервалы синхронизации
            networkTransform.syncInterval = 0.1f;
   

            // Компрессия
            networkTransform.compressRotation = true;
            networkTransform.positionPrecision = 1000;

            Debug.Log("NetworkTransform configured successfully");
        }
        else
        {
            Debug.LogError("NetworkTransform component not found!");
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        Vector3 move = transform.TransformDirection(movement) * moveSpeed * Time.deltaTime;

        characterController.Move(move);

        // Простая гравитация
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