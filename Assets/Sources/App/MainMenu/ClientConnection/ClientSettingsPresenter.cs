using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSettingsPresenter : MonoBehaviour, ISaveData
{
    [SerializeField]
    private ClientSettingsUI clientSettingsUI;

    private ClientSettingsModel model;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        model = new ClientSettingsModel(this);
    }


    public ConnData GetConnData()
    {
        return model.GetConnData();
    }

    public void Save()
    {
        if (model != null)
        {
            model.SaveData(clientSettingsUI.GetConnData());
        }
    }
}
