
public class ServerSettingsModel
{
    private ISaveLoadConnServerData saveLoadConnServerData;
    private ServerSettingsPresenter presenter;

    public ServerSettingsModel(ServerSettingsPresenter presenter)
    {
        this.presenter = presenter;
        saveLoadConnServerData = SaveLoadDataImpl.Instance;
    }

    public ConnData GetConnData()
    {
        return saveLoadConnServerData.GetConnDataServer();
    }

    public void SaveData(ConnData connData)
    {
        saveLoadConnServerData.SaveConnServerData(connData);
    }
}
