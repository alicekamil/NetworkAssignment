using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AmmoPack : NetworkBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer)
        {
            FiringAction firingAction = other.GetComponent<FiringAction>();
            if (!firingAction) return;
            firingAction.Recharge();


            NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }
}