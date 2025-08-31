using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerToClientPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject networkManager;

    [SerializeField]
    private AddPlayerToClientUI addPlayerToClientUI;

    private AddPlayerToClientModel model;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        model = new AddPlayerToClientModel(networkManager, this);
    }

    public void ConnToServer(GameObject playerPrefab, Transform spawnPos)
    {
        model.ConnToServer(playerPrefab, spawnPos);
    }
    public void OnSuccess(string message)
    {
        addPlayerToClientUI.ShowSuccess(message);
    }
    public void OnFailure(string errorMessage)
    {
        addPlayerToClientUI.ShowFailure(errorMessage);
    }
}
