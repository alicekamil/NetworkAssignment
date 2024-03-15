using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBulletDamage : MonoBehaviour
{
    [SerializeField] int damage = 25;

    void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.transform.GetComponent<Health>();
        if (health == null) return;
        health.TakeDamage(damage);
    }
}