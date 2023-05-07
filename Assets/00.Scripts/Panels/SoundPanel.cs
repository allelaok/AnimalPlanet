using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundPanel : BasePanel
{
    [SerializeField] GameObject bgmContent;
    [SerializeField] BaseButton bgmUp;
    [SerializeField] BaseButton bgmDown;
    [SerializeField] GameObject effectContent;
    [SerializeField] BaseButton effectUp;
    [SerializeField] BaseButton effectDown;
    Image[] bgms;    
    Image[] effects; 

    [SerializeField] BaseButton okBtn;
    [SerializeField] BaseButton exitBtn;

    // Start is called before the first frame update
    void Start()
    {
        okBtn.OnClickMethod.AddListener(OnCLickSoundOKBtn);
        exitBtn.OnClickMethod.AddListener(OnCLickSoundXBtn);

        //bgmUp.OnClickMethod.AddListener(delegate{ OnClick_BGMBtn(true); });
        bgmUp.OnClickMethod.AddListener(delegate{ OnClick_BGMBtn(true); });
        bgmDown.OnClickMethod.AddListener(delegate { OnClick_BGMBtn(false); });
        effectUp.OnClickMethod.AddListener(delegate { OnClick_EffectBtn(true); });
        effectDown.OnClickMethod.AddListener(delegate { OnClick_EffectBtn(false); });

        bgms = bgmContent.GetComponentsInChildren<Image>();
        effects = effectContent.GetComponentsInChildren<Image>();

        for (int i = 0; i < bgms.Length; i++)
        {
            float bgmv = (i + 1) / 5f;
            BaseButton btn = bgms[i].gameObject.AddComponent<BaseButton>();
            btn.scaleEffect = true;
            btn.OnEnterMethod = new UnityEvent();
            btn.OnEnterMethod.AddListener(delegate { SetBGMContents(bgmv); });

            btn.OnClickMethod = new UnityEvent();
            btn.OnClickMethod.AddListener(delegate { SetBGMContents(bgmv); });

            if (i == 0)
            {
                btn.OnExitMethod = new UnityEvent();
                btn.OnExitMethod.AddListener(delegate { SetBGMContents(0); });
            }
        }

        for (int i = 0; i < effects.Length; i++)
        {
            float effectV = (i + 1) / 5f;
            BaseButton btn = effects[i].gameObject.AddComponent<BaseButton>();
            btn.scaleEffect = true;
            btn.OnEnterMethod = new UnityEvent();
            btn.OnEnterMethod.AddListener(delegate { SetEffectContents(effectV); });

            btn.OnClickMethod = new UnityEvent();
            btn.OnClickMethod.AddListener(delegate { SetEffectContents(effectV); });

            if (i == 0)
            {
                btn.OnExitMethod = new UnityEvent();
                btn.OnExitMethod.AddListener(delegate { SetEffectContents(0); });
            }
        }

        if (PlayerPrefs.HasKey("bgmV"))
            bgmV = PlayerPrefs.GetFloat("bgmV");
        else bgmV = 1;

        if (PlayerPrefs.HasKey("effectV"))
            effectV = PlayerPrefs.GetFloat("effectV");
        else
            effectV = 1;

        SetPanel();
    }

    public void SetPanel()
    {
        SetBGMContents(bgmV);
        SetEffectContents(effectV);
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("bgmV", bgmV);
        PlayerPrefs.SetFloat("effectV", effectV);
    }

    float bgmV;
    float effectV;
    public void OnClick_BGMBtn(bool up)
    {
        if (up)
            bgmV += 1f / bgms.Length;
        else
            bgmV -= 1f / bgms.Length;

        bgmV = Mathf.Clamp(bgmV, 0, 1);

        SetBGMContents(bgmV);
    }

    public void OnClick_EffectBtn(bool up)
    {
        if (up)
            effectV += 1f / effects.Length;
        else
            effectV -= 1f / effects.Length;

        effectV = Mathf.Clamp(effectV, 0, 1);

        SetEffectContents(effectV);
    }

    void SetBGMContents(float bgmV)
    {
        this.bgmV = bgmV;
        for (int i = 0; i < bgms.Length; i++)
        {
            if (i < Mathf.Round(bgmV * bgms.Length))
            {
                Color color = bgms[i].color;
                color.a = 1f;
                bgms[i].color = color;
            }
            else
            {
                Color color = bgms[i].color;
                color.a = 0.3f;
                bgms[i].color = color;
            }
            //bgms[i].gameObject.SetActive(i < Mathf.Round(bgmV * bgms.Length));
        }
        SoundManager.instance.SetBGMVolume(bgmV);
    }

    void SetEffectContents(float effectV)
    {
        this.effectV = effectV;
        for (int i = 0; i < effects.Length; i++)
        {
            if(i < Mathf.Round(effectV * effects.Length))
            {
                Color color = effects[i].color;
                color.a = 1f;
                effects[i].color = color;
            }
            else
            {
                Color color = effects[i].color;
                color.a = 0.3f;
                effects[i].color = color;
            }

            //effects[i].gameObject.SetActive(i < Mathf.Round(effectV * effects.Length));
        }
        SoundManager.instance.SetEffectVolume(effectV);
    }

    public void OnCLickSoundXBtn()
    {
        bgmV = PlayerPrefs.GetFloat("bgmV");
        effectV = PlayerPrefs.GetFloat("effectV");
        SetPanel();
        SceneManager.instance.PanelOn(SceneManager.PANEL.home);
    }
    public void OnCLickSoundOKBtn()
    {
        SaveVolume();
        SceneManager.instance.PanelOn(SceneManager.PANEL.home);
    }
}
