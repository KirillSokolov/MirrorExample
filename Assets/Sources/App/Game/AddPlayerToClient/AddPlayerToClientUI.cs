using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddPlayerToClientUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statusConn;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Transform spawnPos;
   
    [SerializeField]
    private AddPlayerToClientPresenter presenter;


    void Start()
    {
        Init();
    }

    private void Init()
    {
        presenter.ConnToServer(playerPrefab, spawnPos);
    }
    public void ShowSuccess(string message)
    {
        statusConn.text = message;
    }
    public void ShowFailure(string message)
    {
        statusConn.text = message;
    }
}
