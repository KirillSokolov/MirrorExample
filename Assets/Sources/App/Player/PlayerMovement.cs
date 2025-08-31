using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    [Header("Sync Variables")]
    [SyncVar(hook = "OnPositionChanged")]
    private Vector3 syncedPosition;

    [SyncVar(hook = "OnRotationChanged")]
    private Quaternion syncedRotation;

    [SyncVar(hook = "OnVelocityChanged")]
    private Vector3 syncedVelocity;

    private Rigidbody rb;
    private bool isInitialized = false;
    private float positionThreshold = 0.1f;
    private float rotationThreshold = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        InitializePlayer();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer)
        {
            InitializePlayer();
        }
    }

    private void InitializePlayer()
    {
        isInitialized = true;

        // Применяем начальные синхронизированные значения
        if (!isLocalPlayer)
        {
            transform.position = syncedPosition;
            transform.rotation = syncedRotation;
            if (rb != null)
            {
                rb.velocity = syncedVelocity;
            }
        }
    }

    private void Update()
    {
        if (!isInitialized) return;

        if (isLocalPlayer)
        {
            HandleInput();
            CheckForChanges();
        }
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
    private void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        // Поворот в направлении движения
        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Применяем движение через Rigidbody для физики
        if (rb != null)
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            transform.position = newPosition;
        }
    }

    private void CheckForChanges()
    {
        // Проверяем изменения позиции
        if (Vector3.Distance(transform.position, syncedPosition) > positionThreshold)
        {
            CmdUpdatePosition(transform.position);
        }

        // Проверяем изменения вращения
        if (Quaternion.Angle(transform.rotation, syncedRotation) > rotationThreshold)
        {
            CmdUpdateRotation(transform.rotation);
        }

        // Проверяем изменения скорости (если используем Rigidbody)
        if (rb != null && Vector3.Distance(rb.velocity, syncedVelocity) > 0.1f)
        {
            CmdUpdateVelocity(rb.velocity);
        }
    }

    [Command]
    private void CmdUpdatePosition(Vector3 newPosition)
    {
        syncedPosition = newPosition;
    }

    [Command]
    private void CmdUpdateRotation(Quaternion newRotation)
    {
        syncedRotation = newRotation;
    }

    [Command]
    private void CmdUpdateVelocity(Vector3 newVelocity)
    {
        syncedVelocity = newVelocity;
    }

    // Хуки для синхронизации
    private void OnPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!isLocalPlayer && isInitialized)
        {
            // Плавное перемещение для других игроков
            if (Vector3.Distance(transform.position, newPos) > 1f)
            {
                StartCoroutine(SmoothMove(newPos));
            }
            else
            {
                transform.position = newPos;
            }
        }
    }

    private void OnRotationChanged(Quaternion oldRot, Quaternion newRot)
    {
        if (!isLocalPlayer && isInitialized)
        {
            transform.rotation = newRot;
        }
    }

    private void OnVelocityChanged(Vector3 oldVel, Vector3 newVel)
    {
        if (!isLocalPlayer && isInitialized && rb != null)
        {
            rb.velocity = newVel;
        }
    }

    private System.Collections.IEnumerator SmoothMove(Vector3 targetPosition)
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 startPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    // Для дебаггинга
    private void OnGUI()
    {
        if (isLocalPlayer)
        {
            GUI.Label(new Rect(10, 10, 200, 20), $"Position: {transform.position}");
            GUI.Label(new Rect(10, 30, 200, 20), $"Is Local Player: {isLocalPlayer}");
        }
    }
}