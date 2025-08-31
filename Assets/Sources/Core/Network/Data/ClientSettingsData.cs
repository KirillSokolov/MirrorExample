using Mirror;
using UnityEngine;

public class ClientSettingsData
{
    public Transform spawnPoint { get; private set; }
    public GameObject playerPrefab { get; private set; }
    public string nickname { get; private set; }
    public ConnData connData { get; private set; }

    public PlayerSpawnMethod playerSpawnMethod { get; private set; } = PlayerSpawnMethod.RoundRobin;
    public HeadlessStartOptions headlessStartMode { get; private set; } = HeadlessStartOptions.DoNothing;

    private ISaveLoadUserData saveLoadUserData = SaveLoadDataImpl.Instance;
    private ISaveLoadConnClientData saveLoadConnClient = SaveLoadDataImpl.Instance;

    public ClientSettingsData(GameObject playerPrefab, Transform spawnPoint)
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
        return saveLoadConnClient.GetConnClientData();
    }
}
