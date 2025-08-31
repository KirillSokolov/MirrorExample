using System.Collections;
using UnityEngine;

public class ServerSettingsPresenter : MonoBehaviour, ISaveData
{
    [SerializeField]
    private ServerSettingsUI serverSettingsUI;

    private ServerSettingsModel model;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        model = new ServerSettingsModel(this);
    }

    public ConnData GetConnData()
    {
        return model.GetConnData();
    }

    public void Save()
    {
        if (model == null)
        {
            model.SaveData(serverSettingsUI.GetConnData());
        }
    }
   
}
