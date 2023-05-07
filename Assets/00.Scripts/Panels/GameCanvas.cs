using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public EndPanel endPnl;
    // Start is called before the first frame update
    public BaseButton leftBtn;
    public BaseButton rightBtn;
    public BaseButton tornadoBtn;

    public void GamePanelOn()
    {
        endPnl.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }   
    
    private void Start()
    {
        leftBtn.OnClickMethod.AddListener(OnClick_leftBtn);
        rightBtn.OnClickMethod.AddListener(OnClick_rightBtn);
        tornadoBtn.OnClickMethod.AddListener(OnClick_tornadoBtn);

    }

    private void OnClick_leftBtn()
    {
        GameManager.instance.OnClickLeftBtn();
    }

    void OnClick_rightBtn()
    {
        GameManager.instance.OnClickRightBtn();

    }

    void OnClick_tornadoBtn()
    {
        GameManager.instance.OnClickTornado();
    }
}
