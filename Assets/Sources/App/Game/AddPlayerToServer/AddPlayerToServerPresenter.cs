using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerToServerPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject networkManager;

    [SerializeField]
    private AddPlayerToServerUI addPlayerToServerUI;

    private AddPlayerToServerModel model;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        model = new AddPlayerToServerModel(networkManager, this);
    }


    public void StartServer(GameObject playerPrefab, Transform spawnPos)
    {
        model.StartServer(playerPrefab, spawnPos);
    }

    public void OnSuccess(string message)
    {
        addPlayerToServerUI.ShowNetworkSuccess(message);
    }

    public void OnFailure(string errorMessage)
    {
        addPlayerToServerUI.ShowNetworkFail(errorMessage);
    }
}
