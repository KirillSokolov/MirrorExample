using kcp2k;
using Mirror;
using UnityEngine;

public class ServerManager : NetworkManager
{
    private ServerSettingsData settingsData;
    private INetworkConnListener networkConnListener;

    public void Configurate(ServerSettingsData settingsData, INetworkConnListener networkConnListener)
    {
        this.networkConnListener = networkConnListener;
        this.settingsData = settingsData;
        playerSpawnMethod = settingsData.playerSpawnMethod;
        headlessStartMode = settingsData.headlessStartMode;
        playerPrefab = settingsData.playerPrefab;
        this.transport = GetComponent<KcpTransport>();
        networkAddress = settingsData.connData.ipAddress;
        (this.transport as KcpTransport).port = (ushort)settingsData.connData.port;
        gameObject.SetActive(true);
        StartHost();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        maxConnections = settingsData.maxPlayers;
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        networkConnListener.OnSuccess(settingsData.connData.ipAddress);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (playerPrefab == null)
        {
            networkConnListener.OnFailure("Player prefab is not set!");
            return;
        }

        Vector3 spawnPosition = settingsData.spawnPoint.position;
        Quaternion spawnRotation = settingsData.spawnPoint.rotation;

        GameObject player = Instantiate(playerPrefab, spawnPosition, spawnRotation);

        player.name = settingsData.nickname;

        NetworkServer.AddPlayerForConnection(conn, player);
    }
}