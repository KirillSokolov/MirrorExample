using TMPro;
using UnityEngine;

public class ServerSettingsUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ipAddressText, portText;
  
    [SerializeField]
    private ServerSettingsPresenter presenter;

    void Start()
    {
        InitUI(presenter.GetConnData());
    }
    void InitUI(ConnData connData)
    {
        portText.text = connData.port.ToString();
        ipAddressText.text = connData.ipAddress;
    }

    public ConnData GetConnData()
    {
       return new ConnData(ipAddressText.text, int.Parse(portText.text));
    }

}
