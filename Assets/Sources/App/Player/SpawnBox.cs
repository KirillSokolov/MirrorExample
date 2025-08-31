using UnityEngine;
using Mirror;
using System.Collections;

public class SpawnBox : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletLifetime = 3f;

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Всегда регистрируем префаб на клиенте при старте
        if (bulletPrefab != null)
        {
            try
            {
                NetworkClient.RegisterPrefab(bulletPrefab);
                Debug.Log($"✅ Registered bullet prefab: {bulletPrefab.name}");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"⚠️ Prefab registration warning: {e.Message}");
            }
        }
    }

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

        // Создаем пулю
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);

        // Настраиваем физику
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bullet.transform.forward * bulletSpeed;
        }

        // Спавним на всех клиентах
        NetworkServer.Spawn(bullet);

        // Уничтожаем через время
        StartCoroutine(DestroyBulletAfterLifetime(bullet));
    }

    private IEnumerator DestroyBulletAfterLifetime(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifetime);

        if (bullet != null)
        {
            NetworkServer.Destroy(bullet);
        }
    }

    // Клиентский выстрел для немедленной обратной связи
    [Client]
    public void ClientShoot()
    {
        // Немедленный визуальный эффект на клиенте
        if (bulletPrefab != null)
        {
            GameObject localBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = localBullet.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = localBullet.transform.forward * bulletSpeed;
            Destroy(localBullet, bulletLifetime);
        }

        // Вызываем команду на сервере
        CmdSpawn();
    }
}