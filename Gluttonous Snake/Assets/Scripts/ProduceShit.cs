using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProduceShit : MonoBehaviour
{
    //单例
    private ProduceShit() { }
    public static ProduceShit Instance;

    public float TestInterval=0.01f;
    public int Xlimit = 21;//左右限定步数
    public int Ylimit = 11;//上下限定步数
    public int Xoffset = 11;//偏移

    public GameObject ShitGameObject;//道具
    public GameObject RewardGameObject;//奖品

    public Sprite[] PropSprites;//道具图片
    private Transform _shitHolder;




    void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        _shitHolder = GameObject.Find("ShitHolder").transform;
        MakeShit(false);
    }
	
	void Update () {
	
	}



    public void MakeShit(bool isReward)
    {
        //下标随机
        int index = Random.Range(0, PropSprites.Length);

        //实例化
        GameObject shit = Instantiate(ShitGameObject);

        //给道具加上图片
        shit.GetComponent<Image>().sprite = PropSprites[index];

        //设置父物体（父物体，是否保持世界坐标）
        shit.transform.SetParent(_shitHolder,false);

        //范围内，上下左右随机步数
        int x = Random.Range(-Xlimit + Xoffset, Xlimit);
        float y = Random.Range(-Ylimit , Ylimit);

        //设置位置信息
        shit.transform.localPosition=new Vector3(x*30,y*30,0);

        if (isReward)
        {
            //实例化
            GameObject reward = Instantiate(RewardGameObject);

            //设置父物体（父物体，是否保持世界坐标）
            reward.transform.SetParent(_shitHolder, false);

            //范围内，上下左右随机步数
            x = Random.Range(-Xlimit + Xoffset, Xlimit);
            y = Random.Range(-Ylimit, Ylimit);

            //设置位置信息
            reward.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        }
    }
}
