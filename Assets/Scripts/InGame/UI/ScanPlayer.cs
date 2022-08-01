using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanPlayer : MonoBehaviour
{
    public Enemy enemy;
    BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        col = GetComponent<BoxCollider2D>();
    }
}
