using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Aim : NetworkBehaviour {
    
    // references
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform turretPivot;

    [Header("Settings")]
    [SerializeField] private float turningRate = 30f;

    private Vector2 previousMovementInput2;

    // subscribe to aim event when object is spawned in
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            return;
        }
        inputReader.AimEvent += HandleMove;
    }

    // unsubscribe from aim event when object is despawned
    public override void OnNetworkDespawn() {
        if (!IsOwner) {
            return;
        }
        inputReader.AimEvent -= HandleMove;
    }
    
    private void Update() {
        if (!IsOwner) {
            return;
        }
        // calculate rotation input based on previous momement
        float rotationInput = previousMovementInput2.x * -turningRate * Time.deltaTime;
        // rotate turret pivot
        turretPivot.Rotate(0f, 0f, rotationInput);
    }

    // handle aim movement input
    private void HandleMove(Vector2 movementInput2) {
        // update previous movement input for aiming
        previousMovementInput2 = movementInput2;
    }
}
