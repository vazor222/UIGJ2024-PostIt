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
    public float tableYPosition;


    private Vector3 pickupOffset;
    private bool isBeingDragged = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        tableYPosition = GameplaySceneManager.Instance.GetTablePos();
        UpdateSortingOrder();
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
        rb.simulated = false;
    }

    #region mouse controls
    public void OnStartDrag()
    {
        Debug.Log("package clicked, starting drag");
        isBeingDragged = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pickupOffset = transform.position - mousePosition;
    }

    public void OnDrag()
    {
        if (isBeingDragged)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            if (mousePosition.y < tableYPosition) {
                mousePosition.y = tableYPosition;
            }
            transform.position = mousePosition + pickupOffset;
        }
    }

    public void OnendDrag()
    {
        isBeingDragged = false;

        Vector3 currentPosition = transform.position;
        if (currentPosition.y < tableYPosition)
        {
            currentPosition.y = tableYPosition;
        }
        transform.position = currentPosition;

        rb.simulated = true;
        //TODO Place in box
    }
    #endregion
}
