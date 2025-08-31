using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSettingsData 
{
    public int maxPlayers { get; private set; } = Constants.MAX_PLAYERS_SERVER;
    public string serverPassword { get; private set; } = "";
    public Transform spawnPoint { get; private set; }
    public GameObject playerPrefab { get; private set; }
    public string nickname { get; private set; }
    public ConnData connData { get; private set; }

    public PlayerSpawnMethod playerSpawnMethod { get; private set; } = PlayerSpawnMethod.RoundRobin;
    public HeadlessStartOptions headlessStartMode { get; private set; } = HeadlessStartOptions.AutoStartServer;

    private ISaveLoadUserData saveLoadUserData = SaveLoadDataImpl.Instance;
    private ISaveLoadConnServerData saveLoadConnServer = SaveLoadDataImpl.Instance;

    public ServerSettingsData(GameObject playerPrefab, Transform spawnPoint)
    {
        this.spawnPoint = spawnPoint;
        this.playerPrefab = playerPrefab;
        connData = GetConnData();
        nickname = GetNickname();

    }
    private string GetNickname()
    {
        return saveLoadUserData.GetNickname();
    }

    private ConnData GetConnData()
    {
        return saveLoadConnServer.GetConnDataServer();
    }
}
