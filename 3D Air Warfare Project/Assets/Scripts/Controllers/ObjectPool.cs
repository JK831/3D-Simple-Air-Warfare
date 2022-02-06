using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    [SerializeField]
    private GameObject poolingObjectPrefab;
    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();
    private void Awake()
    {
        Instance = this;
        Initialize(2000);
    }
    private void Initialize(int initCount)
    { 
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
        Debug.Log("Bullet Created");
    }
    private GameObject CreateNewObject()
    {   GameObject newObj = Managers.Resource.Instantiate($"Bullet");
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }
    public static GameObject GetObject()
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        if (Instance.gameObject != null)
            obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }

}
