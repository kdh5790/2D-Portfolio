using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public Transform spawnPos;
    public bool isComplete;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isComplete)
        {
            StartCoroutine(FindObjectOfType<Lerp>().WallCloseEvent());
            StartCoroutine(SpawnBoss());
        }
    }

    IEnumerator SpawnBoss()
    {
        while (!isComplete)
        {
            if (isComplete)
                break;
            yield return null;
        }

        if (isComplete)
        {
            var boss = Instantiate(Resources.Load("���� �θ�"), spawnPos.position, Quaternion.identity, transform.parent);
            boss.name = "���� �θ�";
            yield break;
        }
    }
}
