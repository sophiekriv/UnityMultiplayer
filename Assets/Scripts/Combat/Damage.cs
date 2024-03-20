using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Damage : MonoBehaviour
{
    [SerializeField] private int damage = 5; // amount of damage

    AudioManager audioManager; // audio manager for sound effects

    private void Awake() {
        // find audio manager in scene
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private ulong ownerClientId; // client ID of owner
    // set owner of object
    public void SetOwner(ulong ownerClientId) {
        this.ownerClientId = ownerClientId;
    }

    // called when collider enters trigger
    private void OnTriggerEnter2D(Collider2D col) {
        // if no rigidbody return
        if(col.attachedRigidbody == null) {
            return;
        }
        // if other collider has network object
        if (col.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj)) {
            //if owner of object is same as owner of other object
            if(ownerClientId == netObj.OwnerClientId) {
                return;
            }
        }
        //if other collider has health component
        if (col.attachedRigidbody.TryGetComponent<Health>(out Health health)) {
            // deal damage
            health.TakeDamage(damage);
            // play sound effect
            audioManager.PlaySFX(audioManager.boom);
        }
    }
}
