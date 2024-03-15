using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthPack : NetworkBehaviour
{
    [SerializeField] GameObject healthPackPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer)
        {
            Health health = other.GetComponent<Health>();
            if (!health) return;
            health.AddHealth(25);

            int xPosition = Random.Range(-4, 4);
            int yPosition = Random.Range(-2, 2);

            // Client instantiation.
            GameObject newHealthPack = Instantiate(healthPackPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
            // Network instantiation.
            NetworkObject no = newHealthPack.GetComponent<NetworkObject>();
            no.Spawn();
            
            
            NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }
}