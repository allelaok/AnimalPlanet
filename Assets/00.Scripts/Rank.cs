using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rank : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text rank;
    [SerializeField]
    TMPro.TMP_Text nickName;

    [SerializeField]
    TMPro.TMP_Text score;
    //[SerializeField]
    //Image profile;

    [SerializeField]
    Sprite circle;
    [SerializeField]
    Image[] img;
    Color blue;
    private void Start()
    {
        ColorUtility.TryParseHtmlString("#002F6C", out blue);
    }
    public void SetInfo(RankInfo rankInfo = null)
    {
        if (rankInfo == null)
        {
            rank.text = string.Empty;
            nickName.text = string.Empty;
            score.text = string.Empty;
            //profile.sprite = circle;
        }
        else
        {
            rank.text = rankInfo.rank.ToString();
            nickName.text = rankInfo.nickName;
            score.text = rankInfo.score.ToString();

            if(rankInfo.nickName == GameManager.instance.nickName)
            {
                for(int i = 0; i < img.Length; i++)
                {
                    img[i].color = blue;
                    rank.color = Color.white;
                    nickName.color = Color.white;
                }
            }
            else
            {
                for (int i = 0; i < img.Length; i++)
                {
                    img[i].color = Color.white;
                    rank.color = blue;
                    nickName.color = blue;
                }
            }
            //profile.sprite = SceneManager.instance.GetProfileSprite(rankInfo.selectedCharacter);
        }
    }

}
