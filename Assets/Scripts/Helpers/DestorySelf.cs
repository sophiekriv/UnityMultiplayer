using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// method to destroy game Object
public class DestorySelf : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) 
    {
        Destroy(gameObject);
    }
}
