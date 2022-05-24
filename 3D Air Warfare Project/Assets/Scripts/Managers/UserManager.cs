using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager
{
    int _playerScore;

    public int PlayerScore
    {
        get { return _playerScore; }
        set { _playerScore = value; }
    }

    internal void Clear()
    {
        _playerScore = 0;
    }
}
