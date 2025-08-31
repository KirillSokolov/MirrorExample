using UnityEngine;

public class AddPlayerToClientModel: INetworkConnListener
{
    private GameObject networkManager;
    private AddPlayerToClientPresenter presenter;

    public AddPlayerToClientModel(GameObject networkManager, AddPlayerToClientPresenter presenter)
    {
        this.networkManager = networkManager;
        this.presenter = presenter;
    }

    public void ConnToServer(GameObject playerPrefab, Transform spawnPos)
    {
        var clientManager = InstanceClientManager();
        clientManager.Configurate(new ClientSettingsData(playerPrefab, spawnPos), this);
    }

    private ClientManager InstanceClientManager()
    {
        networkManager.AddComponent<ClientManager>();
        var clientManager = networkManager.GetComponent<ClientManager>();
        return clientManager;
    }

    public void OnFailure(string errorMessage)
    {
        presenter.OnFailure(errorMessage);
    }

    public void OnSuccess(string message)
    {
        presenter.OnSuccess($"Connected to: {message}");
    }


}
