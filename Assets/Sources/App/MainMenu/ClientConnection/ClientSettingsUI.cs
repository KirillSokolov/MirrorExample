using TMPro;
using UnityEngine;

public class ClientSettingsUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI portText;

    [SerializeField]
    private TMP_InputField ipAddressText;
   
    [SerializeField]
    private ClientSettingsPresenter presenter;

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
