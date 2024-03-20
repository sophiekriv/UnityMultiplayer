using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    // lifetime of game object
    [SerializeField] private float lifetime = 1f;
    private void Start()
    {
        // destro game object after specified lifetime
        Destroy(gameObject, lifetime);
    }
}
