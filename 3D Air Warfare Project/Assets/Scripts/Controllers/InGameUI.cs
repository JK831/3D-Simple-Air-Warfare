using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public int score = 0;
    public float speed = 30.0f;
    UI_Explanation ui;
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

    public InGameUI()
    {

        hp = 100;
    }

    public void Init()
    {

    }

    public void GameOver()
    {
        state = GameState.GameOver;
        ui.gameObject.SetActive(false);
        targetUI.gameObject.SetActive(false);

        //endingUI = Managers.UI.ShowSceneUI<Ending_UI>();
        
 
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


    public void SetSpeed(float speed)
    {
        ui.UpdateSpeed(speed);
    }

    public void SetScore(int score)
    {
        ui.UpdateScore(score);
    }

    public void SetHP(int hp)
    {
        ui.UpdateHP(hp);
    }

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {


        if (state == GameState.GameOver || state == GameState.Victory)
        {
            return;
        }
        if (target == null)
            return;


        if (state == GameState.Ing && IsVisibleToCamera(target.transform))
        {
            targetUI.gameObject.SetActive(true);
        }
        else
            targetUI.gameObject.SetActive(false);
    }


}
