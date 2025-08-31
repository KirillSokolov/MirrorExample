using TMPro;
using UnityEngine;

public class AddPlayerToServerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statusConn;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Transform spawnPos;

    [SerializeField]
    private AddPlayerToServerPresenter presenter;


    void Start()
    {
        Init();
    }

    private void Init()
    {
        presenter.StartServer(playerPrefab, spawnPos);
    }
    public void ShowNetworkSuccess(string message)
    {
        statusConn.text = message;
    }
    public void ShowNetworkFail(string message)
    {
        statusConn.text = message;
    }
}
