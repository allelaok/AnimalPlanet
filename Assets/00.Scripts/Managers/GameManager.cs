using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public enum STATE
{
    Defualt,
    Ready,
    Play,
    CamMove,
    Tornado,
    Subway,
    GameOver,
    Pop
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public float BaseSpeed { get { return baseSpeed; } }
    public float Interval { get { return interval; } }
    public float Pow { get { return pow; } }
    public int TornadoCnt { get { return tornadoCnt; } }
    public int Price { get { return price; } }
    public float IntervalSpeed { get { return intervalSpeed; } }

    [HideInInspector]
    public float Speed;
    [HideInInspector]
    public int BestScore;


    [SerializeField]
    TMPro.TMP_Text scoreText;
    int score;
    [HideInInspector]
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    }

    [HideInInspector]
    public string nickName;
    [HideInInspector]
    public int selectedCharacter;
    [HideInInspector]
    public string characters;
    [HideInInspector]
    public int coin;
    [HideInInspector]
    public List<Task> tasks = new List<Task>();

    [SerializeField]
    int tornadoCnt = 3;
    [SerializeField]
    float interval = 2f;
    [SerializeField]
    float baseSpeed = 5;
    [SerializeField]
    int price = 1;
    [SerializeField]
    float intervalSpeed = 0.1f;
    [SerializeField]
    float pow = 0.95f;



    [SerializeField]
    Transform[] springPositions;
    Dictionary<Transform, Transform[]> springPos = new Dictionary<Transform, Transform[]>();


    Dictionary<Transform, int> before = new Dictionary<Transform, int>();
    [SerializeField]
    Transform[] springs;

    FirebaseManager FMng;
    SceneManager SceneMng;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < springs.Length; i++)
        {
            if (springPos.ContainsKey(springs[i]) == false)
            {
                before.Add(springs[i], -1);
                springPos.Add(springs[i], springPositions[i].GetComponentsInChildren<Transform>());
                SetSpring(springs[i]);
            }
        }

        FMng = FirebaseManager.instance;
       SceneMng = SceneManager.instance;

        SoundManager.instance.BGM((int)Sound.Lobby_BGM);

        SceneMng.LoadingPanelOn();

        if (FMng.InternetOn())
        {
#if UNITY_EDITOR
            //FMng.GuestLogIn();
#elif UNITY_IOS
            FMng.GameCenterLogin();
#elif UNITY_ANDROID
            FMng.GoogleLogin();
#endif
        }
        else
        {
            // 네트워크 오류 팝업
            SceneManager.instance.PanelOn(SceneManager.PANEL.home);
            SceneManager.instance.Popup("인터넷 연결 안됨");
        }

    }

  

    [SerializeField] int tmpBestScore;
    [SerializeField] string tmpNickName;
#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if (string.IsNullOrEmpty(tmpNickName))
            //    nickName = tmpNickName;
            //BestScore = tmpBestScore;
            //FirebaseManager.instance.CheckRank();

            FirebaseManager.instance.LogOut();
        }
    }
#endif

    public void InitData()
    {
        BestScore = 0;
        selectedCharacter = 0;
        characters = "0";
        coin = 0;
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        int setWidth = 1284; // 사용자 설정 너비
        int setHeight = 2278; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

    public void SetSpring(Transform spring)
    {
        int idx = Random.Range(1, springPos[spring].Length);
        while (before.ContainsValue(idx))
        {
            idx = Random.Range(1, springPos[spring].Length);
        }

        spring.position = springPos[spring][idx].position;
        spring.rotation = Quaternion.identity;
        before[spring] = idx;
    }




    [HideInInspector]
    public bool turnLeft;
    [HideInInspector]
    public bool turnRight;
    [HideInInspector]
    public bool tornado;
    public void OnClickLeftBtn()
    {
        turnLeft = true;
    }
    public void OnClickRightBtn()
    {
        turnRight = true;
    }
    public void OnClickTornado()
    {
        tornado = true;
    }


    public TMPro.TMP_Text[] nickName_t;
    bool setNickname;
    public void SetNickname()
    {
        if (setNickname) return;
        if (string.IsNullOrEmpty(nickName)) return;

        print("nickname");
        for (int i = 0; i < nickName_t.Length; i++)
        {
            nickName_t[i].text = nickName;
        }
        setNickname = true;
    }
}
