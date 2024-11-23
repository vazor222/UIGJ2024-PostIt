using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Table : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameplaySceneManager.Instance.TableCollider = this.GetComponent<Collider2D>();
    }
}
