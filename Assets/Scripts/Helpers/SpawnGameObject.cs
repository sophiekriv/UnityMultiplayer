using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGameObject : MonoBehaviour
{
    // prefab to be spwaned in when object destroyed
    [SerializeField] private GameObject prefab;

    private void OnDestroy() {
        // instantiate prefab at current position
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
