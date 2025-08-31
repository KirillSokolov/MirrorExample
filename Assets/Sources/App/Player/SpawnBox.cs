using Mirror;
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

        Vector3 spawnPosition = firePoint.transform.position + transform.forward;
        Quaternion spawnRotation = firePoint.rotation;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);

        bullet.name = $"Bullet_{netId}_{Time.time}";

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = bullet.transform.forward * bulletSpeed;
        }

        NetworkServer.Spawn(bullet);

        Destroy(bullet, bulletLifetime);
    }
}
