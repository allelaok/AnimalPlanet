using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvas : MonoBehaviour
{
   public enum PANEL
    {
        home,
        ranking,
        setting,
        sound,
        //account,
        logout,
        setNickName
    }

    public BasePanel[] panels;

    public void PanelOn(PANEL panel)
    {
        if ((int)panel < panels.Length)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].gameObject.SetActive(i == (int)panel);
            }
        }
        else
        {
            Debug.Log("no panel");
        }
    }
}

