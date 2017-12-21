using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Text LasText;
    public Text BesText;
    public Button StartButton;
    public Toggle DogToggle;
    public Toggle CatToggle;
    public Toggle HitWallToggle;
    public Toggle HitSelfToggle;


    void Awake()
    {
        LasText.text = "上次\n\n长度：" + PlayerPrefs.GetInt("lastlong", 0) + "\n\n分数：" + PlayerPrefs.GetInt("lastscore", 0);
        BesText.text = "最好\n\n长度：" + PlayerPrefs.GetInt("bestlong", 0) + "\n\n分数：" + PlayerPrefs.GetInt("bestscore", 0);

    }



    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void Start()
    {
        StartButton.onClick.AddListener(StartGame);//绑定方法

        if (PlayerPrefs.GetString("dog", "Dog") == "Dog")
        {
            DogToggle.isOn = true;
            PlayerPrefs.SetString("dog", "Dog");
            PlayerPrefs.SetString("dog1", "Dog01");
            PlayerPrefs.SetString("dog2", "Dog02");
        }
        else
        {
            CatToggle.isOn = true;
            PlayerPrefs.SetString("dog", "Cat");
            PlayerPrefs.SetString("dog1", "Cat01");
            PlayerPrefs.SetString("dog2", "Cat02");
        }

        if (PlayerPrefs.GetInt("hit",1)==1)
        {
            HitWallToggle.isOn = true;
            PlayerPrefs.SetInt("hit",1);
        }
        else
        {
            HitSelfToggle.isOn = true;
            PlayerPrefs.SetInt("hit",0);
        }
    }

    void Update()
    {

    }


    /// <summary>
    /// 选择狗
    /// </summary>
    public void SelectedDog(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("dog", "Dog");
            PlayerPrefs.SetString("dog1", "Dog01");
            PlayerPrefs.SetString("dog2", "Dog02");
        }
    }



    /// <summary>
    /// 选择猫
    /// </summary>
    public void SelectedCat(bool isOn)
    {
        PlayerPrefs.SetString("dog", "Cat");
        PlayerPrefs.SetString("dog1", "Cat01");
        PlayerPrefs.SetString("dog2", "Cat02");

    }



    /// <summary>
    /// 怼墙
    /// </summary>
    public void HitWall(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("hit",1);

        }

    }



    /// <summary>
    /// 怼自己
    /// </summary>
    public void HitSelf(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("hit", 0);

        }
    }

}

