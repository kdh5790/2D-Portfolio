using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineRange : MonoBehaviour
{
    public PolygonCollider2D map1;
    public PolygonCollider2D map2;
    CinemachineConfiner confiner;

    private void Awake()
    {
        confiner = GetComponent<CinemachineConfiner>();
    }

    public void ChangeCollider(int num)
    {

        switch(num)
        {
            case 1:
                confiner.m_BoundingShape2D = map1;
                break;
            case 2:
                confiner.m_BoundingShape2D = map2;
                break;
        }

    }
}
