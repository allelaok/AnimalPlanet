using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLoadingPanel : BasePanel
{
    [SerializeField] BaseButton homeBtn;
    void Start()
    {
        homeBtn.OnClickMethod.AddListener(OnClick_HomeBtn);

    }
    void OnClick_HomeBtn()
    {
        for (int i = GameManager.instance.tasks.Count - 1; i >= 0; i--)
        {
            if (GameManager.instance.tasks[i] != null)
                GameManager.instance.tasks[i].Dispose();
        }
        GameManager.instance.tasks.Clear();

        SoundManager.instance.BGM((int)Sound.Lobby_BGM);
        SceneManager.instance.PanelOn(SceneManager.PANEL.home);
    }
}
