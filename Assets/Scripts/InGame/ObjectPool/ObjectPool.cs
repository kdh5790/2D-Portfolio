using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Poolable poolObj;
    public int allocateCount;

    Archer player;

    private Stack<Poolable> poolStack = new Stack<Poolable>();

    void Start()
    {
        player = FindObjectOfType<Archer>();
        if (player != null)
            Allocate();
    }

    public void Allocate()
    {
        for (int i = 0; i < allocateCount; i++)
        {
            Poolable allocateObj = Instantiate(poolObj, player.transform.position, Quaternion.identity);
            allocateObj.Create(this);
            poolStack.Push(allocateObj);
        }
    }

    public GameObject Pop()
    {
        Poolable obj = poolStack.Pop();
        obj.gameObject.SetActive(true);
        return obj.gameObject;
    }

    public void Push(Poolable obj)
    {
        obj.gameObject.transform.position = Vector3.zero;
        obj.gameObject.SetActive(false);
        poolStack.Push(obj);
    }
}
