using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager
{
    int _enemyNum = 1;
    bool _victory = false;

    public int RemainEnemy
    {
        get { return _enemyNum; }
        set { _enemyNum = value; }
    }

    public bool Victory
    {
        get { return _victory; }
        set { _victory = value; }
    }
    public void Init()
    {
        // 파일 읽어와서 씬 별로 적의 수 추가
    }

    public void GameOver()
    {
        Managers.UI.ShowPopupUI<Ending_UI>();
    }

    internal void Clear()
    {
        _enemyNum = 1;
        _victory = false;
    }
}
