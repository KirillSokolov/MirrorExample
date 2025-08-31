
public class MainMenuModel
{
    private const string SERVER = "Server", CLIENT = "Client";

    private ISaveLoadUserData saveLoadUserData;
    private ISaveLoadConnTypeData saveLoadConnTypeData;

    private NavigationUI navigationUI;

    private int connTogglePos = 0;

    private MainMenuPresenter presenter;

    public MainMenuModel(MainMenuPresenter presenter, NavigationUI navigationUI)
    {
        this.presenter = presenter;
        this.navigationUI = navigationUI;
        saveLoadUserData = SaveLoadDataImpl.Instance;
        saveLoadConnTypeData = SaveLoadDataImpl.Instance;
    }

    public void SaveData(string nickname)
    {
        saveLoadUserData.SaveNickname(nickname);
        saveLoadConnTypeData.SaveConnType(connTogglePos);
    }
    public string GetSavedNickname()
    {
        return saveLoadUserData.GetNickname();
    }

    public void GoToPlay()
    {
        InterfaceFinderInactive.FindObjectsOfInterfaceIncludingInactive<ISaveData>().ForEach(it =>
        {
            it.Save();
        });
        navigationUI.OpenPlayScreen();
    }
    public void OnToggleSelected(string name)
    {
        switch (name)
        {
            case SERVER:
                presenter.ShowServerUI();
                connTogglePos = 0;
                break;
            case CLIENT:
                presenter.ShowClientUI();
                connTogglePos = 1;
                break;
            default:
                break;
        }

    }
}
