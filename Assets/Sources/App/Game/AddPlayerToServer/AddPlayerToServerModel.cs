using UnityEngine;

public class AddPlayerToServerModel : INetworkConnListener
{
    private GameObject networkManager;
    private AddPlayerToServerPresenter presenter;

    public AddPlayerToServerModel(GameObject networkManager, AddPlayerToServerPresenter presenter)
    {
        this.networkManager = networkManager;
        this.presenter = presenter;
    }
    public void StartServer(GameObject playerPrefab, Transform spawnPos)
    {
        var serverManager = InstanceServerManager();
        serverManager.Configurate(new ServerSettingsData(playerPrefab, spawnPos), this);
    }

    private ServerManager InstanceServerManager()
    {
        networkManager.AddComponent<ServerManager>();
        var serverManager = networkManager.GetComponent<ServerManager>();
        return serverManager;
    }

    public void OnSuccess(string message)
    {

        presenter.OnSuccess($"Online server: {message}");
    }

    public void OnFailure(string errorMessage)
    {
        presenter.OnFailure(errorMessage);
    }
}
