using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetNickNamePanel : BasePanel
{

    [SerializeField] TMP_InputField nickname;
    [SerializeField] TMP_Text state;
    [SerializeField] BaseButton checkBtn;
    [SerializeField] BaseButton okBtn;
    [SerializeField] BaseButton exitBtn;

    Color red;
    Color blue;

    // Start is called before the first frame update
    void Start()
    {
        ColorUtility.TryParseHtmlString("#DE5541", out red);
        ColorUtility.TryParseHtmlString("#002F6C", out blue);

        checkBtn.OnClickMethod.AddListener(OnClickCheckBtn);
        okBtn.OnClickMethod.AddListener(OnClickOkBtn);
        exitBtn.OnClickMethod.AddListener(OnClickExitBtn);
        nickname.onValueChanged.AddListener(ChangeText);

        okBtn.parent.gameObject.SetActive(false);
        //this.state.text = "Check Nickname";
        //this.state.color = blue;

    }

    public override void SetActice(bool on)
    {
        nickname.text = "";
        checkBtn.parent.gameObject.SetActive(true);
        state.text = "Check Nickname";
        this.state.color = blue;
        base.SetActice(on);
    }

    void ChangeText(string text)
    {
        if (text.Length > 10)
            nickname.text = text.Remove(10);
        else
        {
            checkBtn.parent.gameObject.SetActive(true);
            okBtn.parent.gameObject.SetActive(false);
        }
    }

    void OnClickCheckBtn() 
    {
        FirebaseManager.instance.CheckNickName(nickname.text, State);
    }

    void State(bool avaliable)
    {
        if (avaliable)
        {
            this.state.text = "Avaliable";
            this.state.color = blue;
        }
        else
        {
            this.state.color = red;
            this.state.text = "Unavaliable";
        }

        checkBtn.parent.gameObject.SetActive(!avaliable);
        okBtn.parent.gameObject.SetActive(avaliable);
    }

    void OnClickOkBtn()
    {
        SceneManager.instance.PanelOn(SceneManager.PANEL.loading);
        FirebaseManager.instance.afterSend = SceneManager.PANEL.home;
        GameManager.instance.nickName = nickname.text;
        GameManager.instance.SetNickname();
        FirebaseManager.instance.SendData("nickName", nickname.text, 1);
    }

    void OnClickExitBtn()
    {
        SceneManager.instance.PanelOn(SceneManager.PANEL.home);
    }

}
