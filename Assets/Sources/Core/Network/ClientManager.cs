using kcp2k;
using Mirror;
using System;
using UnityEngine;

public class ClientManager : NetworkManager
{
    private ClientSettingsData settingsData;
    private INetworkConnListener networkConnListener;
    public void Configurate(ClientSettingsData settingsData, INetworkConnListener networkConnListener)
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

        StartClient();
    }
     
    public override void OnStopServer()
    {
        base.OnStopServer();
        networkConnListener.OnFailure("🛑 Server stopped!");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);

        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            networkConnListener.OnFailure($"❌ Server full! Max players: {maxConnections}");
            return;
        }
        networkConnListener.OnSuccess(networkAddress);
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

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        networkConnListener.OnFailure($"🔌 Client disconnected: {conn.connectionId}");
    }


    public override void OnClientConnect()
    {
        base.OnClientConnect();

        Debug.Log("✅ Connected to server successfully!");
        networkConnListener.OnSuccess(networkAddress);

        if (NetworkClient.connection != null)
        {
        }
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        networkConnListener.OnFailure("❌ Disconnected from server!");

    }

    public void OnClientError(Exception exception)
    {
        networkConnListener.OnFailure($"💥 Client error: {exception.Message}");
    }

 }