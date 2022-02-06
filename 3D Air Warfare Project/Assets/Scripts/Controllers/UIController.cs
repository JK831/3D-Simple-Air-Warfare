using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : UI_Base
{
    public int score = 0;
    public float speed = 30.0f;
    UI_Button ui;
    Ending_UI endingUI;
    public TargetUI targetUI;
    public string title;
    public GameObject target;
    public int hp;

    bool isGameOver = false;

    enum GameState
    {
        GameOver,
        Victory,
        Ing,
    }


    GameState state = GameState.Ing;

    public static UIController ui_Instance;

    public static UIController Instance
    {
        get
        {
            initialize();
            return ui_Instance;
        }
    }

    public void GameOver()
    {
        state = GameState.GameOver;
        ui.gameObject.SetActive(false);
        targetUI.gameObject.SetActive(false);

        endingUI = Managers.UI.ShowSceneUI<Ending_UI>();
        
 
    }

    public void Victory()
    {
        state = GameState.Victory;
        ui.gameObject.SetActive(false);
        targetUI.gameObject.SetActive(false);

        title = "Victory";
        endingUI.gameObject.SetActive(true);

    }


    bool IsVisibleToCamera(Transform transform)
    {
        Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }


    void Start()
    {
        initialize();

        ui = Managers.UI.ShowSceneUI<UI_Button>();
        hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
#endif
        }

        if (state == GameState.GameOver || state == GameState.Victory)
        {
            return;
        }
        if (target == null)
            return;

        ui.UpdateSpeed(speed);
        ui.UpdateScore(score);
        ui.UpdateHP(hp);

        if (state == GameState.Ing && IsVisibleToCamera(target.transform))
        {
            targetUI.gameObject.SetActive(true);
        }
        else
            targetUI.gameObject.SetActive(false);
    }

    static void initialize()
    {
        GameObject go = GameObject.Find("@UIController");
        if (go == null)
        {
            go = new GameObject { name = "@UIController" };
            go.AddComponent<UIController>();
        }
        DontDestroyOnLoad(go);
        ui_Instance = go.GetComponent<UIController>();
    }
}
