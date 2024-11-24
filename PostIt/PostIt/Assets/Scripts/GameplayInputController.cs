using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayInputController : MonoBehaviour
{
    public GameObject DraggedObject = null;
    public Camera mainCamera;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("clicked mousePos : " + mousePos);
            //RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            foreach (Package package in
            GameplaySceneManager.Instance.PackagesOnTable()) {
                if (package.GetComponent<Collider2D>() == Physics2D.OverlapPoint(mousePos))
                {
                    Debug.Log("hit package");
                }
            }

            /*
            if (!(hit.collider is null) && !(hit.collider.gameObject is null))
            {
                Debug.Log("clicked on Gameobject");
                DraggedObject = hit.collider.gameObject;
            }*/
        }

        if (!(DraggedObject is null))
        {
            //Debug.Log("dragging");
        }

        if (Input.GetMouseButtonUp(0))
        {
            DraggedObject = null;
        }
    }
}

