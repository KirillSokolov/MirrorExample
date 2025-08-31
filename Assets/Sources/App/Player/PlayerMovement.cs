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

    // ������������ ��� �������� ��������
    [SerializeField] private float positionLerpSpeed = 15f;
    [SerializeField] private float rotationLerpSpeed = 15f;

    // �������� ��� �������������
    private float lastSyncTime = 0f;
    private const float syncInterval = 0.1f; // 10 ��� � �������

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
        // �������� �����
        isGrounded = characterController.isGrounded;

        // ����� ������������ �������� ��� ������� �����
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        // ��������� �����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ������ ��������
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        moveDirection = move * moveSpeed;

        // ������
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = jumpForce;
        }

        // ����������
        verticalVelocity -= gravity * Time.deltaTime;
        moveDirection.y = verticalVelocity;

        // ���������� ��������
        characterController.Move(moveDirection * Time.deltaTime);

        // ���������� ������������������ ����������
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

        // �������������� ��������� �� ���� ��������
        RpcUpdateMovement(position, rotation, velocity, grounded);
    }

    [ClientRpc]
    private void RpcUpdateMovement(Vector3 position, Quaternion rotation, Vector3 velocity, bool grounded)
    {
        if (isLocalPlayer) return; // �� ��������� � ���������� ������

        syncedPosition = position;
        syncedRotation = rotation;
        syncedVelocity = velocity;
        isGrounded = grounded;
    }

    private void InterpolateMovement()
    {
        // ������������ �������
        transform.position = Vector3.Lerp(transform.position, syncedPosition, positionLerpSpeed * Time.deltaTime);

        // ������������ ��������
        transform.rotation = Quaternion.Lerp(transform.rotation, syncedRotation, rotationLerpSpeed * Time.deltaTime);

        // ��� ��-��������� ������� ����� ��������� ������
        if (!characterController.isGrounded)
        {
            characterController.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
        }
    }

    // ����� ��� �������������� �������������
    [ClientCallback]
    public void ForceSync()
    {
        if (isLocalPlayer)
        {
            CmdUpdateMovement(transform.position, transform.rotation, moveDirection, isGrounded);
        }
    }

    // ���������� ��� ������������ ��� ���������� �������������
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