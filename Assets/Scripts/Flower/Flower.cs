using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Flower : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    protected int flowerValue = 10;

    protected bool alreadyCollected;
    public abstract int Collect();

    // set value of flower
    public void SetValue(int value) {
        flowerValue = value;
    }

    // show or hide flower sprites
    protected void Show(bool show) {
        spriteRenderer.enabled = show;
    }
}
