using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
public class Package : MonoBehaviour
{
    public static int sortingLayerStart = 0;
    private static int currentSortingOrder = 0;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float finalPositionY;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        finalPositionY = GameplaySceneManager.Instance.GetTablePos();
        UpdateSortingOrder();
    }
    private void Update()
    {
        if (transform.position.y <= finalPositionY) {
            Land();
        }
    }
    public void UpdateSortingOrder()
    {
        currentSortingOrder++;
        spriteRenderer.sortingOrder = currentSortingOrder;
    }
    public void Land() {
        rb.simulated = false;
    }

}
