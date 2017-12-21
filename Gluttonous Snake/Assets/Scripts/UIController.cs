using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    //单例
    private UIController() { }
    public static UIController Instance;

    public bool IsPause = false;
    public int Score = 0;
    public int Length = 0;
    public Text MessageText;
    public Text ScoreText;
    public Text LengthText;
    public Image BgImage;
    private Color _bgColor;
    public Image PauseImage;
    public Sprite[] PauseSprites;
    public Button PauseButton;
    public Button HomeButton;
    public bool IsHit = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PauseButton.onClick.AddListener(OnPause);
        HomeButton.onClick.AddListener(Home);

        if (PlayerPrefs.GetInt("hit",1)==0)
        {
            IsHit = false;
            foreach (Transform item in BgImage.gameObject.transform)//遍历父物体下的所有子物体
            {
                item.GetComponent<Image>().enabled = false;
            }
        }
    }

    void Update()
    {
        switch (Score/100)
        {
            case 0:
            case 1:
                break;
            case 2:
            case 3:
                ColorUtility.TryParseHtmlString("#F0F8FF", out _bgColor);
                BgImage.color = _bgColor;
                MessageText.text = "等级：" + 2;
                break;
            case 4:
            case 5:
                ColorUtility.TryParseHtmlString("#00FFFF", out _bgColor);
                BgImage.color = _bgColor;
                MessageText.text = "等级：" + 3;
                break;
            case 6:
            case 7:
                ColorUtility.TryParseHtmlString("#7FFF00", out _bgColor);
                BgImage.color = _bgColor;
                MessageText.text = "等级：" + 4;
                break;
            case 8:
            case 9:
                ColorUtility.TryParseHtmlString("#5F9EA0", out _bgColor);
                BgImage.color = _bgColor;
                MessageText.text = "等级：" + 5;
                break;
            case 10:
                ColorUtility.TryParseHtmlString("#000000", out _bgColor);
                BgImage.color = _bgColor;
                MessageText.text = "无尽模式";
                break;
        }
    }



    /// <summary>
    /// 暂停
    /// </summary>
    public void OnPause()
    {
        IsPause = !IsPause;
        if (IsPause)
        {
            Time.timeScale = 0;//暂停
            PauseImage.sprite = PauseSprites[1];
        }
        else
        {
            Time.timeScale = 1;//暂停
            PauseImage.sprite = PauseSprites[0];
        }
    }



    /// <summary>
    /// 加分规则
    /// </summary>
    /// <param name="score"></param>
    /// <param name="length"></param>
    public void GameUi(int score=5,int length=1)
    {
        Score += score;
        Length += length;
        ScoreText.text = "得分：\n" + Score;
        LengthText.text = "长度：\n" + length;
    }




    /// <summary>
    /// 返回主页
    /// </summary>
    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);//加载0号场景
    }

}

