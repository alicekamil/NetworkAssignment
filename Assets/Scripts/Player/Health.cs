using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(100);


    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;
    }

    public void AddHealth(int health)
    {
        currentHealth.Value += health;
    }

    public void TakeDamage(int damage){
        damage = damage<0? damage:-damage;
        currentHealth.Value += damage;
    }

}
