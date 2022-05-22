using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool s_instance;
    [SerializeField]
    private GameObject poolingObjectPrefab;
    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();

    static ObjectPool Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }

    public static void Init()
    {
        GameObject go = GameObject.Find("@ObjectPool");
        if (go == null)
        {
            go = new GameObject { name = "@ObjectPool" };
            go.AddComponent<ObjectPool>();
        }
        //DontDestroyOnLoad(go);
        s_instance = go.GetComponent<ObjectPool>();
    }
    private void Awake()
    {
        //Init();
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
