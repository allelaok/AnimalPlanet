using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePanel : BasePanel
{
    public BaseButton rankingBtn;
    public BaseButton soundBtn;
    public BaseButton playBtn;
    public BaseButton howToBtn;
    private void Start()
    {
        rankingBtn.OnClickMethod.AddListener(OnCliCk_RankingBtn);
        soundBtn.OnClickMethod.AddListener(OnClick_SoundBtn);
        playBtn.OnClickMethod.AddListener(OnClick_playBtn);
        howToBtn.OnClickMethod.AddListener(OnClick_howToBtn);
    }

    private void OnClick_howToBtn()
    {
        SceneManager.instance.PanelOn(SceneManager.PANEL.howTo);
    }

    void OnCliCk_RankingBtn()
    {
        SceneManager.instance.LoadingPanelOn();
        FirebaseManager.instance.GetRankInfo2();
    }

    void OnClick_SoundBtn()
    {

        SceneManager.instance.PanelOn(SceneManager.PANEL.sound);
    }

    void OnClick_playBtn()
    {
        SceneManager.instance.GamePanelOn();
        Player.state = STATE.Ready;
    }
}
