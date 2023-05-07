using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanel : BasePanel
{
    [SerializeField] TMPro.TMP_Text score;
    [SerializeField] TMPro.TMP_Text best;

    [SerializeField] BaseButton retryBtn;
    [SerializeField] BaseButton homeBtn;

    // Start is called before the first frame update
    void Start()
    {
        retryBtn.OnClickMethod.AddListener(OnClick_RetryBtn);
        homeBtn.OnClickMethod.AddListener(OnClick_HomeBtn);

    }

    private void OnEnable()
    {
        score.text = GameManager.instance.Score.ToString();
        best.text = GameManager.instance.BestScore.ToString();
    }
    void OnClick_HomeBtn()
    {
        SoundManager.instance.BGM((int)Sound.Lobby_BGM);
        SceneManager.instance.PanelOn(SceneManager.PANEL.home);
    }

    private void OnClick_RetryBtn()
    {
        SceneManager.instance.GamePanelOn();
        Player.state = STATE.Ready;
    }
}
