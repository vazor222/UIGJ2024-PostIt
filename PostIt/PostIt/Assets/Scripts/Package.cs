using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
public class Package : MonoBehaviour
{
    //visuals:
    [SerializeField]
    private SpriteRenderer Color;

    [SerializeField]
    private SpriteRenderer SymbolTopBackground;
    [SerializeField]
    private SpriteRenderer SymbolTop;

    [SerializeField]
    private SpriteRenderer SymbolMiddleBackground;
    [SerializeField]
    private SpriteRenderer SymbolMiddle;

    [SerializeField]
    private SpriteRenderer SymbolBottomBackground;
    [SerializeField]
    private SpriteRenderer SymbolBottom;

    public Destination Destination
    {
        get; private set;
    }
    public Destination SecretDestination
    {
        get; private set;
    }

    private static int currentSortingOrder = 10;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public float tableYPosition;
    private bool inSlot = false;


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
    public void SetIntendedDestination(Sprite DestinationColor, Sprite Symbol, int row, Destination destination, Destination secretDestination = Destination.none) {
        Destination = destination;
        Color.sprite = DestinationColor;
        SymbolTopBackground.gameObject.SetActive(false);
        SymbolMiddleBackground.gameObject.SetActive(false);
        SymbolBottomBackground.gameObject.SetActive(false);
        switch (row) { 
            case 0:
                SymbolTopBackground.gameObject.SetActive(true);
                SymbolTop.sprite = Symbol;
                break;
            case 1:
                SymbolMiddleBackground.gameObject.SetActive(true);
                SymbolMiddle.sprite = Symbol;
                break;
            case 2:
                SymbolBottomBackground.gameObject.SetActive(true);
                SymbolBottom.sprite = Symbol;
                break;
        }
        SecretDestination = secretDestination;
    }

    public void UpdateSortingOrder()
    {
        setSortingOrder(currentSortingOrder);
        currentSortingOrder += 4;
    }

    private void setSortingOrder(int order) {
        order += 1;
        spriteRenderer.sortingOrder = order;
        order += 1;
        Color.sortingOrder = order;
        order += 1;
        SymbolTopBackground.sortingOrder = order;
        SymbolMiddleBackground.sortingOrder = order;
        SymbolBottomBackground.sortingOrder = order;

        order += 1;
        SymbolTop.sortingOrder = order;
        SymbolMiddle.sortingOrder = order;
        SymbolBottom.sortingOrder = order;
    }

    public void ReduceSortOrder()
    {
        if(spriteRenderer.sortingOrder <= 2) { 
            gameObject.SetActive(false);
            return;
        }
        setSortingOrder(spriteRenderer.sortingOrder - 1);

    }

    public bool isOnTable() {
        return !isBeingDragged && 
            rb.gravityScale == 0 
            && !inSlot;
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
        setSortingOrder(5);
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
                transform.position = currentPosition;
                gameObject.layer = 6;
                Land();
                inSlot = true;
                GameplaySceneManager.Instance.HandleMailPlacedInSlot(mailSlot.type, this);
                return;
            }
        }
        inSlot = false;
        rb.gravityScale = gravity;
        transform.position = currentPosition;
        UpdateSortingOrder();
        gameObject.layer = 0;
    }
    #endregion
}
