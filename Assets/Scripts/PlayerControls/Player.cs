using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class Player : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [Header("Settings")]
    [SerializeField] private int ownerPriority = 15;
    
    // call when object spawned on network
    public override void OnNetworkSpawn() {
        if (IsOwner) {
            // set priority of virtual camera for owner
            virtualCamera.Priority = ownerPriority;
        }
    }
}
