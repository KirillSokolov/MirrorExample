using Mirror;
using UnityEngine;

public abstract class PlayerControllerBase : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SyncVar] public float moveSpeed = 5f;
    [SyncVar] public float rotationSpeed = 180f;

    [Header("Components")]
    public NetworkTransformReliable networkTransform;
    protected CharacterController characterController;

    protected Vector3 movement;

    public virtual void Start()
    {
        InitializeComponents();

        if (isLocalPlayer)
        {
            SetupLocalPlayer();
        }
    }

    protected virtual void InitializeComponents()
    {
        characterController = GetComponent<CharacterController>();

        if (networkTransform == null)
            networkTransform = GetComponent<NetworkTransformReliable>();
    }

    protected virtual void SetupLocalPlayer()
    {
        SetupCamera();
    }

    protected abstract void SetupCamera();

    public virtual void Update()
    {
        if (!isLocalPlayer) return;

        HandleInput();
        HandleMovement();
        HandleRotation();
    }

    protected virtual void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, 0, vertical);
    }

    protected abstract void HandleMovement();

    protected virtual void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log($"Local player started: {netId}");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log($"Player connected: {netId}");
    }

    // Метод для телепортации
    [Client]
    public virtual void Teleport(Vector3 position)
    {
        if (isServer)
        {
            transform.position = position;
        }
        else
        {
            CmdTeleport(position);
        }
    }

    [Command]
    protected virtual void CmdTeleport(Vector3 position)
    {
        transform.position = position;
    }

 

    protected virtual Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-10f, 10f), 1f, Random.Range(-10f, 10f));
    }
}