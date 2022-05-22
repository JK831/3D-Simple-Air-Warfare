using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Explanation : UI_Scene
{
    Player _player;
    PlayerController _playerController;

    enum Texts
    {
        SpeedText,
        ScoreText,
        HPText,
        ControlGuide,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    private void Start()
    {
        init();
        GameObject player = GameObject.Find("Player(Clone)");
        _player = player.GetComponent<Player>();
        _playerController = player.GetComponent<PlayerController>();
    }

    public override void init()
    {
        base.init();

       
        Bind<Text>(typeof(Texts));
       
    }


    public void UpdateScore(int score)
    {
        GetText((int)Texts.ScoreText).text = $"Score: {score}";
    }

    public void UpdateSpeed(float _speed)
    {
        GetText((int)Texts.SpeedText).text = $"Speed: {_speed}";
    }

    public void UpdateHP(int hp)
    {
        Debug.Log("hp");
        GetText((int)Texts.HPText).text = $"HP: {hp}";
    }

    private void Update()
    {
        GetText((int)Texts.ScoreText).text = $"Score: {Managers.User.PlayerScore}";
        GetText((int)Texts.SpeedText).text = $"Speed: {_playerController.Speed}";
        GetText((int)Texts.HPText).text = $"HP: {_player.HP}";
    }
}
