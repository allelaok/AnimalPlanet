using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIContent : MonoBehaviour
{
    [SerializeField] Image playerImg;
    [SerializeField] GameObject selectedImg;
    [SerializeField] GameObject selectBtn;
    [SerializeField] GameObject buyPanel;

    [SerializeField] int num;
    // Start is called before the first frame update
    void Start()
    {
        //playerImg.sprite = SceneManager.instance.GetProfileSprite(num, "Player");
    }

   public void SetContent()
    {
        if (GameManager.instance.characters.Contains(num.ToString()))
        {
            buyPanel.SetActive(false);
            if (GameManager.instance.selectedCharacter == num)
            {
                selectBtn.SetActive(false);
                selectedImg.SetActive(true);
            }
            else
            {
                selectedImg.SetActive(false);
                selectBtn.SetActive(true);

            }
        }
        else
        {
            selectedImg.SetActive(false);
            selectBtn.SetActive(false);
            buyPanel.SetActive(true);
        }
    }

    //public void OnClick_Select()
    //{
    //    SceneManager.instance.SelectPlayer(num);
    //}

    //public void OnClick_BuyBtn()
    //{
    //    SceneManager.instance.BuyPlayer(num);
    //}
}
