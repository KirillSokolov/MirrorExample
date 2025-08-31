public class ClientSettingsModel
{
    private ClientSettingsPresenter presenter;

    private ISaveLoadConnClientData saveLoadConnClientData;

    public ClientSettingsModel(ClientSettingsPresenter presenter)
    {
        this.presenter = presenter;
        saveLoadConnClientData = SaveLoadDataImpl.Instance;
    }


    public ConnData GetConnData()
    {
        return saveLoadConnClientData.GetConnClientData();
    }

    public void SaveData(ConnData connData)
    {
        saveLoadConnClientData.SaveConnClientData(connData);
    }
}
