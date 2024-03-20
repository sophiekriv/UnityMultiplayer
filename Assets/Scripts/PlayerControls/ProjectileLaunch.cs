using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLaunch : NetworkBehaviour {
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;
    AudioManager audioManager;

    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    [Header("Settings")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float muzzleFlashDuration;

    private bool shouldFire;
    private float previousFireTime;
    private float muzzleFlashTimer;
    
    // when object spawned on network
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            return;
        }
        // subscribe to primary fire event
        inputReader.PrimaryFireEvent += HandleFire;
    }

    // when object despawned on network
    public override void OnNetworkDespawn() {
        if (!IsOwner) {
            return;
        }
        inputReader.PrimaryFireEvent -= HandleFire;
    }
    
    private void Update()
    {
        // check if muzzle flash is active
        if (muzzleFlashTimer > 0f) {
            muzzleFlashTimer -= Time.deltaTime;
            // deactive muzzle flash when timer runs out
            if (muzzleFlashTimer <= 0f) {
                muzzleFlash.SetActive(false);
            }
        }

        if (!IsOwner) {
            return;
        }
        if (!shouldFire) {
            return;
        }
        // check if enough time has passed since last fire to fire
        if(Time.time < (1 / fireRate) + previousFireTime) {
            return;
        }

        // call server to fire projectile
        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);

        // spawn projectile for client
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);

        // play sound effect
        audioManager.PlaySFX(audioManager.shoot);
        
        // update previous fire time
        previousFireTime = Time.time;

    }
    
    private void HandleFire(bool shouldFire) {
        // update flag for firing
        this.shouldFire = shouldFire;
    }

    // rpc method to spawn server projectile
    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction) {
        // instantiate server projectile at spawn position
        GameObject projectileInstance = Instantiate(
            serverProjectilePrefab,
            spawnPos,
            Quaternion.identity);
        projectileInstance.transform.up = direction;
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        // set owner of projectile
        if (projectileInstance.TryGetComponent<Damage>(out Damage dealDamage)) {
            dealDamage.SetOwner(OwnerClientId);
        }

        // apply velocity to projectile
        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) {
            rb.velocity = rb.transform.up * projectileSpeed;
        }

        // spawn dummy for client
        SpawnDummyProjectileClientRpc(spawnPos, direction);
    }

    [ClientRpc]

    // RPC method to spawn client projectile
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction) {
        // check if object is owner
        if (IsOwner) {
            return;
        }
        // spawn dummy for client
        SpawnDummyProjectile(spawnPos, direction);

    }
    
    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction) {
        // activate flash effect
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        // instantiate client projectile at spawn position
        GameObject projectileInstance = Instantiate(
            clientProjectilePrefab,
            spawnPos,
            Quaternion.identity);
        projectileInstance.transform.up = direction;

        // ignore collision 
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        // apply velocity to projectile
        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
    }

    // when collision detected
    private void OnCollisionEnter2D(Collision2D collision) {
        // play sound effect
        Debug.Log("Collision Detected");
        if (collision.gameObject.tag == "Wall") {
            // Play the wall collision sound effect
            audioManager.PlaySFX(audioManager.wallTouch);
        }
    }
}
