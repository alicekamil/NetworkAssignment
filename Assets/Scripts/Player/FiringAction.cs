using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FiringAction : NetworkBehaviour
{
    public NetworkVariable<int> ammoCount = new NetworkVariable<int>(10);
    public NetworkVariable<float> ammoTimer = new NetworkVariable<float>(1);
    public NetworkVariable<bool> isCoolDown = new NetworkVariable<bool>(false);

    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject clientSingleBulletPrefab;
    [SerializeField] GameObject serverSingleBulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;


    public override void OnNetworkSpawn()
    {
        playerController.onFireEvent += Fire;
    }

    private void Fire(bool isShooting)
    {
        if (isShooting && ammoCount.Value > 0 && !isCoolDown.Value)
        {
            ShootLocalBullet();
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        GameObject bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        ShootBulletClientRpc();
        ammoCount.Value--;
        isCoolDown.Value = true;
        ammoTimer.Value = 1;
    }

    private void Update()
    {
        if (!IsServer) return;

        if (isCoolDown.Value)
        {
            if (ammoTimer.Value <= 0)
            {
                isCoolDown.Value = false;
            }
            
            ammoTimer.Value -= Time.deltaTime;
        }
    }

    [ClientRpc]
    private void ShootBulletClientRpc()
    {
        if (IsOwner) return;
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
    }

    private void ShootLocalBullet()
    {
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

        ShootBulletServerRpc();
    }

    public void Recharge()
    {
        ammoCount.Value = 10;
    }
}