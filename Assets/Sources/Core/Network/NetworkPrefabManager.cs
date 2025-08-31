using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPrefabManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> networkPrefabs = new List<GameObject>();

    private void Awake()
    {
        RegisterAllPrefabs();
    }

    public void RegisterAllPrefabs()
    {
        foreach (GameObject prefab in networkPrefabs)
        {
            RegisterPrefab(prefab);
        }
    }

    public bool RegisterPrefab(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Attempted to register null prefab");
            return false;
        }

        if (prefab.GetComponent<NetworkIdentity>() == null)
        {
            Debug.LogError($"Prefab {prefab.name} is missing NetworkIdentity component");
            return false;
        }

        NetworkManager networkManager = NetworkManager.singleton;
        if (networkManager == null)
        {
            return false;
        }

        // Добавляем, если еще нет в списке
        if (!networkManager.spawnPrefabs.Contains(prefab))
        {
            networkManager.spawnPrefabs.Add(prefab);
            Debug.Log($"Registered prefab: {prefab.name}");
            return true;
        }

        return false;
    }

}