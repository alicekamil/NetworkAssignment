using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shield : NetworkBehaviour
{
    private void Start()
    {
        GetComponent<Health>().currentHealth.OnValueChanged += OnHealthChanged;
    }

    private void OnHealthChanged(int previousvalue, int newvalue)
    {
        if (newvalue <= 0)
        {
            DeathRpc();
        }
    }

    [Rpc(SendTo.Everyone)]
    private void DeathRpc()
    {
        Destroy(gameObject);
    }
}