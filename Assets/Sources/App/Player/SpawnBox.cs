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

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bullet.transform.forward * bulletSpeed;
        }

        NetworkServer.Spawn(bullet);

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

    [Client]
    public void ClientShoot()
    {
        if (bulletPrefab != null)
        {
            GameObject localBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = localBullet.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = localBullet.transform.forward * bulletSpeed;
            Destroy(localBullet, bulletLifetime);
        }
        CmdSpawn();
    }
}