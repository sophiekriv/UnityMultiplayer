using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour {
    
    // references
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;

    // settings
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float turningRate = 30f;

    private Vector2 previousMovementInput;

    // reference to audio manager
    AudioManager audioManager;

    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // subscribe to move event when object is spawned
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            return;
        }
        inputReader.MoveEvent += HandleMove;
    }

    // unsubscribe to move event when object is despawned
    public override void OnNetworkDespawn() {
        if (!IsOwner) {
            return;
        }
        inputReader.MoveEvent -= HandleMove;
    }

    private void Update() {
        if (!IsOwner) {
            return;
        }
        // calculate z-axis rotation based on previous movement
        float zRotation = previousMovementInput.x * -turningRate * Time.deltaTime;
        // rotate body transform
        bodyTransform.Rotate(0f, 0f, zRotation);
        
        // check if movement input is not zero and movement audio is not playing
        if (previousMovementInput != Vector2.zero && !audioManager.SFXSource.isPlaying) {
            audioManager.PlaySFX(audioManager.movement); // play the audio 
        }  
        // check if movement input is zero and movement audio is playing
        else if (previousMovementInput == Vector2.zero && audioManager.SFXSource.isPlaying) {
            // stop audio
            audioManager.StopSFX();
        }
    }

    private void FixedUpdate() {
        if(!IsOwner) {
            return;
        }
        // set velocity of rigidbody
        rb.velocity = (Vector2)bodyTransform.up * previousMovementInput.y * movementSpeed;
    }

    // handle movement input
    private void HandleMove(Vector2 movementInput) {
        // update previous movement input
        previousMovementInput = movementInput;
    }
}
