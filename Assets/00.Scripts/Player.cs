using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]    TMPro.TMP_Text bestScoreText;
    [SerializeField]    Image[] tornadoImg;
    [SerializeField]    Transform subwayPool;
    [SerializeField]    Transform tornado;
    [SerializeField]    Image arrow;
    [SerializeField]    GameObject camMoveBG;
    [SerializeField]    Transform heartPositions;
    [SerializeField]    SpriteRenderer animSprite;
    [SerializeField]    GameObject dieSprite;
   
    public Image[] lifeImg;
    int lifeCnt;

    public Animator anim;
  public static int inverse = 1;
  public List<Vector3> points = new List<Vector3>();

  public static  List<Transform> hearts = new List<Transform>();
  public static  STATE state;

    Camera cam;
    int dieCnt;
    int tornadoCnt;
    //int score;
    int point = 1;
    List<int> targetIndx = new List<int>();
    Transform[] subways;
    GameObject heartPool;
    Vector3 initSize;
    Transform nowTarget;
    [HideInInspector]
    public Transform target;
    Transform[] heartPos;
    int beforePosIndx = 0;
    int idx;
    float inverseTime = 5f;
    float inversedTime;

    [SerializeField] Transform controller;

    private void Start()
    {
        state = STATE.Defualt;
        //animSprite = GetComponentInChildren<SpriteRenderer>();
        //PlayerAndHeart.Add(transform);
        inversedTime = inverseTime;
        heartPos = heartPositions.GetComponentsInChildren<Transform>();
        subways = subwayPool.GetComponentsInChildren<Transform>();
        cam = GetComponentInChildren<Camera>();
        //anim = GetComponentInChildren<Animator>();
        initSize = transform.localScale;
        lifeCnt = lifeImg.Length;
    }
    float invincibilityTime = 3f;
    float invincibilityCurrentTime;
    private void Update()
    {
        switch (state)
        {
            case (STATE.Defualt):
                break;
            case (STATE.Play):
                Move();
                InverseTime();
                SetHeartsPosition(1, 0);
                if (nowTarget)
                {
                    Vector3 dir = nowTarget.position - transform.position;
                    int i = 1;
                    if (dir.x > 0)
                    {
                        i = -1;
                    }
                    float ang = Vector3.Angle(dir, Vector3.up);
                    arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, ang * i));
                }
                if (invincibility)
                {
                    invincibilityCurrentTime += Time.deltaTime;

                    int t = 1;
                    if(Mathf.RoundToInt( 5 * invincibilityCurrentTime) % 2 == 0)
                        t = 0;
                    
                    animSprite.color = new Color(1, 1, 1, t); 

                    if (invincibilityCurrentTime > invincibilityTime)
                    {
                        invincibility = false;
                        invincibilityCurrentTime = 0;
                    }
                }
                break;
            case (STATE.CamMove):
                CamMove();
                break;
            case (STATE.Tornado):
                Tornado();
                break;
            case (STATE.Ready):
                Ready();
                break;
            case (STATE.GameOver):
                GameOver();
                break;
        }
    }
    void GameOver()
    {
        for (int i = 0; i < lifeImg.Length; i++)
        {
            lifeImg[i].enabled = false;
        }

        SoundManager.instance.SoundOneShot(Sound.Wall);
        SoundManager.instance.BGM((int)Sound.Gover_BGM);


        dieSprite.SetActive(true);
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].GetChild(1).gameObject.SetActive(true);
        }

        state = STATE.Defualt;
        SceneManager.instance.PanelOn(SceneManager.PANEL.EndLoading);
        FirebaseManager.instance.SaveScore();
    }
    private void LateUpdate()
    {
        cam.transform.localEulerAngles = -transform.eulerAngles;
    }

    public void Ready()
    {
        dieSprite.SetActive(false);
        SoundManager.instance.BGM((int)Sound.Game_BGM);
        tornado.gameObject.SetActive(false);
        for (int i = 0; i < lifeCnt; i++)
        {
            lifeImg[i].enabled = true;
        }

        for (int i = 0; i < GameManager.instance.TornadoCnt; i++)
        {
            tornadoImg[i].enabled = true;
        }
        initPos();
        DestroyAllHearts();
        CreateHeart();
        tornadoCnt = GameManager.instance.TornadoCnt;
        dieCnt = 0;
        bestScoreText.text = GameManager.instance.BestScore.ToString();
        targetIndx.Clear();
        points.Clear();
        points.Add(transform.position);
        GameManager.instance.Score = 0;
        inverse = 1;
        camMoveBG.SetActive(false);
        GameManager.instance.Speed = GameManager.instance.BaseSpeed;

        enemy.RandomPosition();
        state = STATE.Play;
    }
    [SerializeField] BlackHeart enemy;

    void Move()
    {
        transform.position += transform.up * GameManager.instance.Speed * Time.deltaTime;

        if (target)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.5f)
            {
                target = null;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A) || GameManager.instance.turnLeft)
            {
                transform.rotation *= Quaternion.Euler(new Vector3(0, 0, inverse * 45));
                points.Add(transform.position);

                GameManager.instance.turnLeft = false;
            }
            else if (Input.GetKeyDown(KeyCode.D) || GameManager.instance.turnRight)
            {
                transform.rotation *= Quaternion.Euler(new Vector3(0, 0, -inverse * 45));
                points.Add(transform.position);

                GameManager.instance.turnRight = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space) || GameManager.instance.tornado)
            {
                if (tornadoCnt > 0)
                {
                    tornadoImg[tornadoCnt - 1].enabled = false;
                    points.Clear();
                    state = STATE.Tornado;
                    tornadoCnt--;
                }
                GameManager.instance.tornado = false;
            }

        }
    }

    //[SerializeField] Transform controller;
    void Move2()
    {
        transform.position += transform.up * GameManager.instance.Speed * Time.deltaTime;

        if (target)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.5f)
            {
                target = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) || GameManager.instance.tornado)
        {
            if (tornadoCnt > 0)
            {
                tornadoImg[tornadoCnt - 1].enabled = false;
                points.Clear();
                state = STATE.Tornado;
                tornadoCnt--;
            }
            GameManager.instance.tornado = false;
        }
    }
    void SetHeartsPosition(int pointIdx, int heartIdx, bool pointDis = true, float tmpDis = 0)
    {
        if(points.Count == 0)
        {
            // ???? ???? ???????? ??????
            for (int i = 0; i < hearts.Count; i++)
                hearts[i].position = Vector3.one * -1;
        }
        else if (points.Count == 1)
        {
            float dis = Vector3.Distance(transform.position, points[0]);
            float gap = 0;
            // ???????? ???????? ???? ???? ????
            for (int i = 0; i < hearts.Count; i++)
            {
                gap += GameManager.instance.Interval * Mathf.Pow(GameManager.instance.Pow, i + 1);
                if (dis > gap)
                {
                    hearts[i].position = transform.position - transform.up * gap;
                    hearts[i].up = transform.up;
                }
            }
        }
        else if (points.Count > pointIdx - 1 && hearts.Count > heartIdx)
        {
            // ???? ???????? ???? ???????? ????
            float dis;
            if (pointDis)
            {
                if (pointIdx == 1)
                    dis = Vector3.Distance(transform.position, points[points.Count - pointIdx]);
                else
                    dis = Vector3.Distance(points[points.Count - pointIdx + 1], points[points.Count - pointIdx]);
            }
            else
            {
                dis = Vector3.Distance(hearts[heartIdx - 1].position, points[points.Count - pointIdx]);
            }

            // ?????? ?????? ????
            if (dis < GameManager.instance.Interval * Mathf.Pow(GameManager.instance.Pow, (heartIdx)) - tmpDis)
            {
                pointIdx++;
                SetHeartsPosition(pointIdx, heartIdx, true, dis + tmpDis);
            }
            // ?????? ?????? ????
            else
            {
                Vector3 pos;
                Vector3 up;
                if (pointDis)
                {
                    if (pointIdx == 1)
                    {
                        pos = Vector3.Lerp(transform.position, points[points.Count - pointIdx], (GameManager.instance.Interval * Mathf.Pow(GameManager.instance.Pow, (heartIdx)) - tmpDis) / dis);
                        up = transform.position - points[points.Count - pointIdx];
                    }
                    else
                    {
                        pos = Vector3.Lerp(points[points.Count - pointIdx + 1], points[points.Count - pointIdx], (GameManager.instance.Interval * Mathf.Pow(GameManager.instance.Pow, (heartIdx)) - tmpDis) / dis);
                        up = points[points.Count - pointIdx + 1] - points[points.Count - pointIdx];

                    }
                }
                else
                {
                    pos = hearts[heartIdx - 1].position - hearts[heartIdx - 1].up * GameManager.instance.Interval * Mathf.Pow(GameManager.instance.Pow, (heartIdx));
                    up = hearts[heartIdx - 1].up;
                }

                // ???? ????
                hearts[heartIdx].position = pos;
                hearts[heartIdx].up = up;
                heartIdx++;
                if (hearts.Count > heartIdx)
                    SetHeartsPosition(pointIdx, heartIdx, false);
                else
                {
                    // ?????? ?????? ????
                    for (int i = points.Count - pointIdx - 1; i > -1; i--)
                    {
                        points.RemoveAt(i);
                    }
                }
            }
        }
    }
    void Tornado()
    {
        if (tornado.gameObject.activeSelf == false)
            tornado.gameObject.SetActive(true);

        transform.Rotate(new Vector3(0,0, 300 * Time.deltaTime));
        transform.localScale -= Vector3.one * Time.deltaTime;
        if (transform.localScale.x < 0.1f)
        {
            transform.localScale = initSize;
            tornado.gameObject.SetActive(false);
            SelectSubway();
        }
    }

    void SelectSubway()
    {
        transform.GetComponentInChildren<SpriteRenderer>().enabled = false;

        int subwayIdx = Random.Range(1, subways.Length);
        if (beforeSubway != null)
        {
            while (beforeSubway == subways[subwayIdx])
            {
                subwayIdx = Random.Range(1, subways.Length);
            }
        }
        beforeSubway = null;
        target = subways[subwayIdx];
        state = STATE.CamMove;
    }

    void CamMove()
    {
        transform.position += (target.position - transform.position).normalized * GameManager.instance.Speed * 10 * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            transform.position = target.position;
            Vector3 ang = target.eulerAngles;
            transform.eulerAngles = ang;
            controller.eulerAngles = ang;

            points.Add(target.position);
            transform.GetComponentInChildren<SpriteRenderer>().enabled = true;

            SoundManager.instance.SoundOneShot(Sound.T_Out);

            state = STATE.Play;
            camMoveBG.SetActive(false);
        }
        else if (Vector3.Distance(transform.position, target.position) < 2f)
        {
            camMoveBG.SetActive(true);
        }
    }

    void initPos()
    {
        int i = Random.Range(1, subways.Length);
        
        inverse = 1;
        target = subways[i];

        state = STATE.Play;
        transform.position = subways[i].position;
        transform.rotation = subways[i].rotation;

        controller.rotation = subways[i].rotation;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target == collision.transform)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Subway") || collision.gameObject.layer == LayerMask.NameToLayer("Stair"))
            {
                    target = null;
                    return;
            }
        }
    }
   public static bool invincibility;
    private Transform beforeSubway;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state != STATE.Play || invincibility) return;
        // ???? ??
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            //DestroyAllHearts();
            //if (nowTarget)
            //    Destroy(nowTarget.gameObject);

            dieCnt++;
            lifeImg[lifeCnt - dieCnt].enabled = false;
            //GameManager.instance.Speed = GameManager.instance.BaseSpeed;
            SoundManager.instance.SoundOneShot(Sound.Wall);

            if (dieCnt < lifeCnt)
            {
                //CreateHeart();
                invincibility = true;
            }
            else
            {
                GameOver();
            }
        }
        // ????
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Heart"))
        {
            //state = STATE.Pop;

            hearts.Add(collision.transform.parent);
            // ???? ????
            GameManager.instance.Score += point;
            GameManager.instance.coin += point;
            //scoreText.text = GameManager.instance.Score.ToString();
            if (hearts.Count < 10)
            {
                GameManager.instance.Speed = GameManager.instance.BaseSpeed + (hearts.Count + 1) * GameManager.instance.IntervalSpeed;
            }
            if (GameManager.instance.Score > GameManager.instance.BestScore)
            {
                GameManager.instance.BestScore = GameManager.instance.Score;
                bestScoreText.text = GameManager.instance.Score.ToString();
            }
            collision.gameObject.SetActive(false);

            CreateHeart();
            CreatePop(collision.transform);
            SoundManager.instance.SoundOneShot(Sound.Pet_pop);

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //DestroyAllHearts();
            //dieCnt++;
            for (int i = 0; i < lifeImg.Length; i++)
            {
                lifeImg[i].enabled = false;
            }
            //GameManager.instance.Speed = GameManager.instance.BaseSpeed;
            //tmp
            SoundManager.instance.SoundOneShot(Sound.Wall);
            GameOver();
        }
        // ????
        else if (collision.gameObject.layer == LayerMask.NameToLayer("TornadoItem"))
        {
            inversedTime = 0;
            inverse = -1;
            GameManager.instance.SetSpring(collision.transform);
            //SoundManager.instance.CollSound(Sound.Wall);
        }
        // ??????
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Tornado"))
        {
            if (target == collision.transform)
            {
                target = null;
                return;
            }
            points.Clear();
            transform.position = collision.transform.position;
            beforeSubway = collision.transform;
            state = STATE.Tornado;
            SoundManager.instance.SoundOneShot(Sound.T_in);
            //SelectSubway(collision.transform);
        }
        // ????
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Stair"))
        {
            if (target == collision.transform)
            {
                target = null;
                return;
            }
            Stair stair = collision.transform.GetComponent<Stair>();
            transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            target = stair.otherStair.transform;
            points.Clear();
            //SoundManager.instance.CollSound(Sound.T_in);

            state = STATE.CamMove;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Poison"))
        {
            inversedTime = 0;
            inverse = -1;
            //anim.SetInteger("Inverse", inverse);
            GameManager.instance.SetSpring(collision.transform);
            SoundManager.instance.SoundOneShot(Sound.Eat_Reverse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state != STATE.Play || invincibility) return;
        // ???? ??
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Pet"))
        {
            //DestroyAllHearts();
            if (nowTarget)
                Destroy(nowTarget.gameObject);

            dieCnt++;
            lifeImg[lifeCnt - dieCnt].enabled = false;
            //GameManager.instance.Speed = GameManager.instance.BaseSpeed;
            SoundManager.instance.SoundOneShot(Sound.Wall);

            if (dieCnt < lifeCnt)
            {
                CreateHeart();
                invincibility = true;
            }
            else
            {
                GameOver();
            }
        }
        // ????
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Heart"))
        {
            state = STATE.Pop;

            hearts.Add(collision.transform.parent);
            // ???? ????
            GameManager.instance.Score += point;
            //scoreText.text = GameManager.instance.Score.ToString();
            if (hearts.Count < 10)
            {
                GameManager.instance.Speed = GameManager.instance.BaseSpeed + (hearts.Count + 1) * GameManager.instance.IntervalSpeed;
            }
            if (GameManager.instance.Score > GameManager.instance.BestScore)
            {
                GameManager.instance.BestScore = GameManager.instance.Score;
                bestScoreText.text = GameManager.instance.Score.ToString();
            }
            collision.gameObject.SetActive(false);

            CreateHeart();
            CreatePop(collision.transform);
            SoundManager.instance.SoundOneShot(Sound.Pet_pop);

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            state = STATE.GameOver;
        }
        // ????
        else if (collision.gameObject.layer == LayerMask.NameToLayer("TornadoItem"))
        {
            inversedTime = 0;
            inverse = -1;
            GameManager.instance.SetSpring(collision.transform);
            //SoundManager.instance.CollSound(Sound.Wall);
        }
        // ??????
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Tornado"))
        {
            if (target == collision.transform)
            {
                target = null;
                return;
            }
            points.Clear();
            transform.position = collision.transform.position;
            beforeSubway = collision.transform;
            state = STATE.Tornado;
            SoundManager.instance.SoundOneShot(Sound.T_in);
            //SelectSubway(collision.transform);
        }
        // ????
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Stair"))
        {
            if (target == collision.transform)
            {
                target = null;
                return;
            }
            Stair stair = collision.transform.GetComponent<Stair>();
            transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            target = stair.otherStair.transform;
            points.Clear();
            //SoundManager.instance.CollSound(Sound.T_in);

            state = STATE.CamMove;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Poison"))
        {
            inversedTime = 0;
            inverse = -1;
            //anim.SetInteger("Inverse", inverse);
            GameManager.instance.SetSpring(collision.transform);
            SoundManager.instance.SoundOneShot(Sound.Eat_Reverse);
        }
    }

    void CreatePop(Transform pet)
    {
        GameObject popAnimPbj = Instantiate(Resources.Load<GameObject>("PopAnim"));
        popAnimPbj.transform.position = pet.position;
        popAnimPbj.transform.rotation = transform.rotation;
        PopAnim popAnim = popAnimPbj.AddComponent<PopAnim>();
        popAnim.pet = pet.parent.gameObject;

        popAnim.first = !(hearts.Count > 1);
    }


    void InverseTime()
    {
        if (inverse == 1) 
        {
            inverseTime = 5f;
            animSprite.color = new Color(1, 1, 1, 1);
            //anim.SetInteger("Inverse", inverse);
            return;
        }

        inversedTime += Time.deltaTime;

        if (inversedTime >= inverseTime)
        {
            animSprite.color = new Color(1, 1, 1, 1);
            inverse = 1;
            //anim.SetInteger("Inverse", inverse);
        }
        else
        {
            if (inversedTime % 0.5f < 0.25f)
                animSprite.color = new Color(1, 1, 1, 0.5f);
            else
                animSprite.color = new Color(1, 1, 1, 1);
            inversedTime += Time.deltaTime;
        }
    }


    void DestroyAllHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            Destroy(hearts[i].gameObject);
        }
        if (nowTarget)
        {
            Destroy(nowTarget.gameObject);
        }
        hearts.Clear();
    }

    void CreateHeart()
    {
        idx = Random.Range(1, heartPos.Length);
        while (idx == beforePosIndx)
        {
            idx = Random.Range(1, heartPos.Length);
        }
        beforePosIndx = idx;

        if (heartPool == null || heartPool.transform.childCount <= 0)
            heartPool = Instantiate(Resources.Load<GameObject>("HeartPool"));

        Transform heartPrefab = heartPool.transform.GetChild(0);
        heartPrefab.SetParent(null, true);
        heartPrefab.position = heartPos[idx].position;
        heartPrefab.rotation = Quaternion.identity;
        nowTarget = heartPrefab;
    }


    public void WalkSound1()
    {
        SoundManager.instance.WalkSound1();
    }

    public void WalkSound2()
    {
        SoundManager.instance.WalkSound2();
    }
}
