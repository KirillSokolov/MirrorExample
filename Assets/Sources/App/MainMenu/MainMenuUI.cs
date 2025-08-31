using TMPro;
using UnityEngine;


public class MainMenuUI : MonoBehaviour, IGetEventToggle
{
    [SerializeField]
    private GameObject clientSettingsUI;

    [SerializeField]
    private GameObject serverSettingsUI;

    [SerializeField]
    private TMP_InputField inputNickname;
   
    [SerializeField]
    private MainMenuPresenter presenter;

    private void Start()
    {
        InitUI(presenter.GetSavedNickname());
    }

    private void InitUI(string nickname)
    {
        inputNickname.text = presenter.GetSavedNickname();
    }

    public string GetNickname()
    {
        return inputNickname.text;
    }

    public void OnClickButtonPlay()
    {
        presenter.GoToPlay();
    }

    public void OnToggleSelected(string name)
    {
        presenter.OnToggleSelected(name);
    }

    public void ShowServerUI()
    {
        serverSettingsUI.SetActive(true);
        clientSettingsUI.SetActive(false);
    }

    public void ShowClientUI()
    {
        serverSettingsUI.SetActive(false);
        clientSettingsUI.SetActive(true);
    }
}
