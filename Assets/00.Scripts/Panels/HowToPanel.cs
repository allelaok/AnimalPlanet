

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPanel : BasePanel
{
    public BaseButton exitBtn;
    // Start is called before the first frame update
    void Start()
    {
        
        exitBtn.OnClickMethod.AddListener(OnClick_ExitBtn);
    }

    void OnClick_ExitBtn()
    {
        SceneManager.instance.PanelOn(SceneManager.PANEL.home);
    }
}
