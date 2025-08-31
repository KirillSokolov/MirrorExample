using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{


    [SerializeField]
    private ChatMessage chatMessage;

    [SerializeField]
    private GameObject addPlayerToServer;

    [SerializeField]
    private GameObject addPlayerToClient;


    ISaveLoadUserData saveLoadUserData = SaveLoadDataImpl.Instance;
    ISaveLoadConnTypeData saveLoadConnType = SaveLoadDataImpl.Instance;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (saveLoadConnType.GetConnType() == 0) {
            addPlayerToServer.SetActive(true);
        }
        else
        {
            addPlayerToClient.SetActive(true);
        }
    }
}
