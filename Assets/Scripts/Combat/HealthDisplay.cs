using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;
    
    // call when object spawned on network
    public override void OnNetworkSpawn() {
        if (!IsClient) {
            return;
        }
        // get current health
        health.CurrentHealth.OnValueChanged += HandleHealthChanged;
        // update health bar image
        HandleHealthChanged(0, health.CurrentHealth.Value);
    }

    // when despawned on network
    public override void OnNetworkDespawn() {
         if (!IsClient) {
            return;
        }
        // unsubscribe from current health
        health.CurrentHealth.OnValueChanged -= HandleHealthChanged;
    }

    // handle changes in health value
    private void HandleHealthChanged(int oldHealth, int newHealth) {
        // update fill amount of health bar image
        healthBarImage.fillAmount = (float)newHealth / health.MaxHealth;
    }
}
