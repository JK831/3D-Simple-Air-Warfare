using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending_UI : UI_Popup
{
    enum Texts
    {
        TitleText,
        TimeText,
        ScoreText,
    }

    enum Buttons
    {
        RestartButton,
        ExitButton,
    }

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();


        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        SetTitle("End");
        SetScore(Managers.User.PlayerScore);
        SetTime(Time.realtimeSinceStartup);
    }

    public void SetTitle(string title)
    {
        GetText((int)Texts.TitleText).text = $"{title}";
    }

    public void SetScore(int score)
    {
        GetText((int)Texts.ScoreText).text = $"Score: {score}";
    }

    public void SetTime(float time)
    {
        GetText((int)Texts.TimeText).text = $"Time: {time}";
    }
}
