using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }

    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    UserManager _user = new UserManager();
    StageManager _stage = new StageManager();

    public static UIManager UI { get { return Instance._ui; } }
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UserManager User { get { return Instance._user; } }
    public static StageManager Stage { get { return Instance._stage; } }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
        _input.OnUpdate();
    }
    static void Init()
    {
        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }
        DontDestroyOnLoad(go);
        s_instance = go.GetComponent<Managers>();
    }

    static void Clear()
    {
        s_instance._ui.Clear();
        s_instance._user.Clear();
        s_instance._stage.Clear();
    }
}
