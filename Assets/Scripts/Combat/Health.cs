using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100; // max health value
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(); // current health value

    private bool isDead;
    public Action<Health> OnDie;

    // called when object spawned on network
    public override void OnNetworkSpawn() {
        // check for server
        if (!IsServer) {
            return;
        }
        // set current health to max
        CurrentHealth.Value = MaxHealth;
    }

    // apply damage to health
    public void TakeDamage(int damageValue) {
        ModifyHealth(-damageValue);
    }

    // restore health
    public void RestoreHealth(int healValue) {
        ModifyHealth(healValue);
    }

    // modify health
    private void ModifyHealth(int value) {
        if (isDead) {
            return;
        }
        // calculate new health value 
        int newHealth = CurrentHealth.Value + value;
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);

        // if health reaches zero, trigger OnDie event
        if(CurrentHealth.Value == 0) {
            OnDie?.Invoke(this);
            isDead = true;
        }
    }

}
