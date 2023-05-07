using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public BaseButton exitBtn;
    [SerializeField] TMPro.TMP_Text content;

    void Awake()
    {
        exitBtn.OnClickMethod.AddListener(OnClick_ExitBtn);
        gameObject.SetActive(false);
    }

    private void OnClick_ExitBtn()
    {
        gameObject.SetActive(false);

    }

    public void SetActive(string content)
    {
        this.content.text = content;
        gameObject.SetActive(true);
    }

}
