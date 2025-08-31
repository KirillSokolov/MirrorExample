using UnityEngine;
using Mirror;

public class PlayerNickname : NetworkBehaviour
{
    [SerializeField] private TextMesh nicknameText;
    private ISaveLoadUserData saveLoadUserData;
    [SyncVar(hook = nameof(UpdateName))]
    private string nickname;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (isLocalPlayer)
        {
            saveLoadUserData = SaveLoadDataImpl.Instance;
            nickname = saveLoadUserData.GetNickname();
            nicknameText.text = nickname;
            SetNickname(nickname);
        }
    }

    public void UpdateName(string oldValue, string newValue)
    {
        nicknameText.text = nickname;
    }

    public void SetNickname(string newNickname)
    {
        if (isServer)
        {
            nickname = newNickname;
            nicknameText.text = newNickname;
            UpdateNameOnAllClients(newNickname);
        }
        else
        {
            CmdSetNickname(newNickname);
        }
    }

    [ClientRpc]
    private void UpdateNameOnAllClients(string newName)
    {
        if (nicknameText != null)
            nicknameText.text = newName;
    }

    [Command]
    private void CmdSetNickname(string nickname)
    {
        gameObject.name = nickname;
        nicknameText.text = nickname;

    }
}
