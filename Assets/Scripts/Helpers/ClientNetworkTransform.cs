using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    // override method when object is spawned on network
    public override void OnNetworkSpawn() {
        // call base method
        base.OnNetworkSpawn();
        // check if client is owner
        CanCommitToTransform = IsOwner;
    }
    
    // override method called every frame
    protected override void Update() {
        // check if client is owner
        CanCommitToTransform = IsOwner;
        // update the base 
        base.Update();
        // check if network manager is valid
        if(NetworkManager != null) {
            // check if client is connected or listening
            if(NetworkManager.IsConnectedClient || NetworkManager.IsListening) {
                // check if client can commit to transform
                if(CanCommitToTransform) {
                    // try to commit to server
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                }
            }
        }
    }

    // determine if client should be authoritatiec
    protected override bool OnIsServerAuthoritative() {
        // should not be
        return false;
    }
}
