using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FlowerCollecter : NetworkBehaviour
{
    // keep track of total collected flowers
    public NetworkVariable<int> TotalFlowers = new NetworkVariable<int>();
    
    // referene to audio manager
    AudioManager audioManager;

    private void Awake() {
        // assign audio manager in scene
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        // check if collided object is flower
        if (!col.TryGetComponent<Flower>(out Flower flower)) {
            return;
        }
        // collect flower and get value
        int flowerValue = flower.Collect();
        // check if on server
        if (!IsServer) {
            return;
        }
        // add flower value to total collected flowers
        TotalFlowers.Value += flowerValue;
        // play flower collection sound effect
        audioManager.PlaySFX(audioManager.flowerCollect);

    }
}
