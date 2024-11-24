using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
public class Package : MonoBehaviour
{
    private static int currentSortingOrder = 10;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public float tableYPosition;


    private Vector3 pickupOffset;
    private bool isBeingDragged = false;
    private float gravity;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        tableYPosition = GameplaySceneManager.Instance.GetTablePos();
        UpdateSortingOrder();
        gravity = rb.gravityScale;
    }
    private void Update()
    {
        if (!isOnTable() && transform.position.y <= tableYPosition) {
            Land();
        }
    }
    public void UpdateSortingOrder()
    {
        currentSortingOrder++;
        spriteRenderer.sortingOrder = currentSortingOrder;
    }

    public bool isOnTable() {
        return !isBeingDragged && !rb.simulated;
    }

    public void Land() {
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
    }

    #region mouse controls
    public void OnMouseDown()
    {
        Debug.Log("package clicked, starting drag");
        isBeingDragged = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pickupOffset = transform.position - mousePosition;
        spriteRenderer.sortingOrder = 5;
    }

    void OnMouseDrag()
    {
        if (isBeingDragged)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.y < tableYPosition) {
                mousePosition.y = tableYPosition;
            }
            mousePosition = mousePosition + pickupOffset;
            foreach (MailSlotMarker mailSlot in GameplaySceneManager.Instance.mailSlotMarkers)
            {
                if (Vector2.Distance(mailSlot.slot.transform.position, mousePosition) <= 1)
                {
                    mousePosition = mailSlot.slot.transform.position;
                }
            }
            transform.position = mousePosition;
            rb.angularVelocity = 0;
        }
    }

    void OnMouseUp()
    {
        isBeingDragged = false;

        Vector3 currentPosition = transform.position;
        if (currentPosition.y < tableYPosition)
        {
            currentPosition.y = tableYPosition;
        }
        foreach (MailSlotMarker mailSlot in GameplaySceneManager.Instance.mailSlotMarkers)
        {
            if (Vector2.Distance(mailSlot.slot.transform.position, currentPosition) <= 1)
            {
                currentPosition = mailSlot.slot.transform.position;
                //TODO inform that this mail is in this slot
                transform.position = currentPosition;
                gameObject.layer = 6;
                Land();
                spriteRenderer.sortingOrder -= 1;
                return;
            }
        }
        rb.gravityScale = gravity;
        transform.position = currentPosition;
        spriteRenderer.sortingOrder = currentSortingOrder;
        gameObject.layer = 0;
    }
    #endregion
}
