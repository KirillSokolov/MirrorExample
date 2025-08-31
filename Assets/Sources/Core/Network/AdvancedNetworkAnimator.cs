using Mirror;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NetworkAnimator))]
public class AdvancedNetworkAnimator : NetworkBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private bool syncAllParameters = true;
    [SerializeField] private string[] syncedParameters;

    private Animator animator;
    private NetworkAnimator networkAnimator;
    private NetworkIdentity networkIdentity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        // Настраиваем NetworkAnimator
        if (networkAnimator != null)
        {
            networkAnimator.animator = animator;
          //  networkAnimator.clientAuthority = networkIdentity.hasAuthority;
        }

        Debug.Log("🎯 Animator authority initialized");
    }

    // Методы для синхронизации конкретных параметров
    public void SyncFloat(string paramName, float value)
    {
        if (HasAuthority())
        {
            CmdSetFloat(paramName, value);
        }
    }

    public void SyncBool(string paramName, bool value)
    {
        if (HasAuthority())
        {
            CmdSetBool(paramName, value);
        }
    }

    public void SyncTrigger(string paramName)
    {
        if (HasAuthority())
        {
            CmdSetTrigger(paramName);
        }
    }

    [Command]
    private void CmdSetFloat(string paramName, float value)
    {
        // Сервер устанавливает значение и синхронизирует
        if (animator != null)
        {
            animator.SetFloat(paramName, value);
        }
        RpcSetFloat(paramName, value);
    }

    [Command]
    private void CmdSetBool(string paramName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(paramName, value);
        }
        RpcSetBool(paramName, value);
    }

    [Command]
    private void CmdSetTrigger(string paramName)
    {
        if (animator != null)
        {
            animator.SetTrigger(paramName);
        }
        RpcSetTrigger(paramName);
    }

    [ClientRpc]
    private void RpcSetFloat(string paramName, float value)
    {
        if (!isServer && animator != null) // Не применяем на сервере повторно
        {
            animator.SetFloat(paramName, value);
        }
    }

    [ClientRpc]
    private void RpcSetBool(string paramName, bool value)
    {
        if (!isServer && animator != null)
        {
            animator.SetBool(paramName, value);
        }
    }

    [ClientRpc]
    private void RpcSetTrigger(string paramName)
    {
        if (!isServer && animator != null)
        {
            animator.SetTrigger(paramName);
        }
    }

    [ContextMenu("Force Sync All")]
    public void ForceSyncAll()
    {
        if (HasAuthority())
        {
            // Синхронизируем все параметры
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Float:
                        SyncFloat(param.name, animator.GetFloat(param.name));
                        break;
                    case AnimatorControllerParameterType.Bool:
                        SyncBool(param.name, animator.GetBool(param.name));
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        // Триггеры синхронизируем только при активации
                        break;
                }
            }
        }
    }

    private bool HasAuthority()
    {
        return networkIdentity != null;// && networkIdentity.hasAuthority;
    }
}