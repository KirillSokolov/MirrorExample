using Mirror;
using System.Collections;
using UnityEngine;

public class SpawnBox : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletLifetime = 3f;

  
    [Command]
    public void CmdSpawn()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("❌ Bullet prefab is not set!");
            return;
        }

        Vector3 spawnPosition = firePoint.position;
        Quaternion spawnRotation = firePoint.rotation;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);
        bullet.name = $"Bullet_{netId}_{Time.time}";

        // Настраиваем пулю на сервере
        SetupBulletOnServer(bullet);

        // Спавним на всех клиентах
        NetworkServer.Spawn(bullet);

        StartCoroutine(DestroyBulletAfterLifetime(bullet, bulletLifetime));
    }

    [Server]
    private void SetupBulletOnServer(GameObject bullet)
    {
        // Настройка velocity на сервере
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = bullet.transform.forward * bulletSpeed;
        }

        // Получаем NetworkIdentity пули
        NetworkIdentity bulletIdentity = bullet.GetComponent<NetworkIdentity>();
        if (bulletIdentity != null)
        {
            // Передаем netId вместо GameObject
            RpcSetupBullet(bulletIdentity.netId, bullet.transform.forward * bulletSpeed);
        }
    }

    [ClientRpc]
    public void RpcSetupBullet(uint bulletNetId, Vector3 velocity)
    {
        // Находим пулю по netId
        if (NetworkClient.spawned.TryGetValue(bulletNetId, out NetworkIdentity bulletIdentity))
        {
            GameObject bullet = bulletIdentity.gameObject;
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = velocity;
            }
        }
    }

    private IEnumerator DestroyBulletAfterLifetime(GameObject bullet, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        if (bullet != null)
        {
            NetworkServer.Destroy(bullet);
        }
    }
}