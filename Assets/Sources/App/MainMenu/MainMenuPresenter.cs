using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using Mirror.BouncyCastle.Security;
using kcp2k;
using Telepathy;

public class MainMenuPresenter : MonoBehaviour, ISaveData
{
    [SerializeField]
    private MainMenuUI mainMenuUI;

    [SerializeField]
    private NavigationUI navigationUI;

    private MainMenuModel model;
    void Awake()
    {
        Init();
    }

    private void Init()
    {
        model = new MainMenuModel(this, navigationUI);
    }

    public void Save()
    {
        model.SaveData(mainMenuUI.GetNickname());
    }

    public string GetSavedNickname()
    {
       return model.GetSavedNickname();
    }

    public void GoToPlay()
    {
        model.GoToPlay();   
    }

    public void OnToggleSelected(string name)
    {
        model.OnToggleSelected(name);
    }
    public void ShowClientUI()
    {
        mainMenuUI.ShowClientUI();
    }
    public void ShowServerUI()
    {
        mainMenuUI.ShowServerUI();
    }


}
