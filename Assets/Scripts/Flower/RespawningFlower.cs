using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningFlower : Flower
{
    // invoke event when flower collected
    public event Action<RespawningFlower> Collected;

    // store previous position of flower 
    private Vector3 previousPosition;

    // check for movement and show flower 
    private void Update() {
        if(previousPosition != transform.position) {
            Show(true);
        }
        previousPosition = transform.position;
    }

    // collect flower
    public override int Collect() {
        // if not on server, don't collect flower
        if (!IsServer) {
            Show(false);
            return 0;
        }

        // if collected, return 0
        if (alreadyCollected) {
            return 0;
        }

        // mark flower as collected
        alreadyCollected = true;
        // invoke collected event
        Collected?.Invoke(this);
        return flowerValue;
    }

    // reset flower so it can be collected
    public void Reset() {
        alreadyCollected = false;
    }
}
