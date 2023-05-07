using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BasePanel
{

    public BaseButton soundBtn;
    public BaseButton changeNickNameBtn;
    public BaseButton homeBtn;

    // Start is called before the first frame update
    void Start()
    {
        soundBtn.OnClickMethod.AddListener(delegate { SceneManager.instance.PanelOn(SceneManager.PANEL.sound); });
        changeNickNameBtn.OnClickMethod.AddListener(delegate { SceneManager.instance.PanelOn(SceneManager.PANEL.setNickName); });
        homeBtn.OnClickMethod.AddListener(delegate { SceneManager.instance.PanelOn(SceneManager.PANEL.home); });
    }
}
