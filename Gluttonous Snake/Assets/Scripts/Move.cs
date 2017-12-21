using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public List<Transform> BodyList = new List<Transform>();//身体的位置信息
    public GameObject BodyGameObject;//身体预设物
    public Sprite[] BodySprites=new Sprite[2];
    private Transform _canvas;
    private bool _isDie = false;

    public float Velocity = 0.35f;//间隔时间速率
    public int Step;//步数
    private int _x;//X轴位移量
    private int _y;//Y轴位移量
    private Vector3 _headPos;//记录头部坐标
    public GameObject DieEffect;

    public AudioClip EatClip;//吃音效
    public AudioClip DieClip;//死亡音效


    void Awake()
    {
        _canvas = GameObject.Find("Canvas").transform;

        GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + PlayerPrefs.GetString("dog", "Dog"));
        BodySprites[0] = Resources.Load<Sprite>("Sprites/" + PlayerPrefs.GetString("dog1", "Dog01"));
        BodySprites[1] = Resources.Load<Sprite>("Sprites/" + PlayerPrefs.GetString("dog2", "Dog02"));
    }



    /// <summary>
    /// 初始化
    /// </summary>
    void Start ()
    {
        //重复执行（方法名，多久开始调用，间隔时间）
        InvokeRepeating("StepMove", 0, Velocity);

        //初始化游戏开始的方向
        _y = Step; _x = 0;
    }
	

    /// <summary>
    /// 更新函数
    /// </summary>
	void Update ()
	{
	    KeyControl();
	}


    /// <summary>
    /// 身体增长
    /// </summary>
    void Grow()
    {
        AudioSource.PlayClipAtPoint(EatClip,Vector3.zero);
        int index = (BodyList.Count % 2 == 0) ? 0 : 1;//三目运算
        GameObject body = Instantiate(BodyGameObject,new Vector3(2000,2000,0),Quaternion.identity)as GameObject;//实例化
        body.GetComponent<Image>().sprite = BodySprites[index];//修改图片
        body.transform.SetParent(_canvas,false);//不更改世界坐标 
        BodyList.Add((body.transform));//加入物体位置信息到数组

    }









    /// <summary>
    /// 移动方式
    /// </summary>
    private void StepMove()
    {
        _headPos = transform.localPosition;//记录头部位置信息

        //给当前头部位置赋一个：新值
        transform.localPosition=new Vector3(_headPos.x+_x,_headPos.y+_y,_headPos.z);
        if (BodyList.Count!=0)
        {
            //BodyList.Last().localPosition = _headPos;//把尾巴挪动到头

            //BodyList.Insert(0, BodyList.Last());//插入最后一个元素，到数组

            //BodyList.RemoveAt(BodyList.Count - 1);//移除最后一个元素

            //双色蛇身体，使用这个方法。
            for (int i = BodyList.Count-2; i >=0; i--)
            {
                BodyList[i + 1].localPosition = BodyList[i].localPosition;
            }
            BodyList[0].localPosition = _headPos;
        }

    }



    /// <summary>
    /// 键盘控制
    /// </summary>
    private void KeyControl()
    {
        //按下空格加速
        if (Input.GetKeyDown(KeyCode.Space)&&UIController.Instance.IsPause==false&&_isDie==false)
        {
            CancelInvoke();//取消重复调用

            InvokeRepeating("StepMove", 0, Velocity - 0.33f);//重新调用，修改间隔速率
        }

        //松开空格减速
        if (Input.GetKeyUp(KeyCode.Space) && UIController.Instance.IsPause == false && _isDie == false)
        {
            CancelInvoke();//取消重复
            InvokeRepeating("StepMove", 0, Velocity);//回归原速率
        }

        //判断上下移动方式/且满足：不能直接180度转向
        //就是当点击W，控制物体向上移动时，需要判断物体此刻是否正在向下运动
        if (Input.GetKey(KeyCode.W) && _y != -Step && UIController.Instance.IsPause == false && _isDie == false)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);//头归正
            _x = 0; _y = Step;//设置移动方向
        }
        if (Input.GetKey(KeyCode.A) && _x != Step && UIController.Instance.IsPause == false && _isDie == false)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            _x = -Step; _y = 0;
        }
        if (Input.GetKey(KeyCode.S) && _y != Step && UIController.Instance.IsPause == false && _isDie == false)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 180);
            _x = 0; _y = -Step;
        }
        if (Input.GetKey(KeyCode.D) && _x != -Step && UIController.Instance.IsPause == false && _isDie == false)
        {
            transform.localRotation = Quaternion.Euler(0, 0, -90);
            _x = Step; _y = 0;
        }
    }


    /// <summary>
    /// 吃道具
    /// </summary>
    private void OnTriggerEnter2D(Collider2D col)
    {
        //判断标签
        if (col.tag=="Shit")
        {
            Destroy(col.gameObject);//销毁Shit物体
            UIController.Instance.GameUi();
            ProduceShit.Instance.MakeShit(Random.Range(0, 100) < 20 ? true : false);//吃掉一个就生成一个
            Grow();
        }
        else if (col.tag=="Reward")
        {
            Destroy(col.gameObject);//销毁Shit物体
            UIController.Instance.GameUi(Random.Range(20,100));
            Grow();
        }
        else if (col.tag=="Body")
        {
            Die();
        }
        else
        {

            if (UIController.Instance.IsHit)
            {
                Die();
            }
            else
            {
                switch (col.name)
                {
                    case "Up":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "Down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                        break;
                    case "Left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 240, -transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "Right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 330, -transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
           
        }

    }



    /// <summary>
    /// 死亡方法
    /// </summary>
    public void Die()
    {
        AudioSource.PlayClipAtPoint(DieClip, Vector3.zero);
        CancelInvoke();//取消移动
        _isDie = true;//死了
        Instantiate(DieEffect);//播放特效
        StartCoroutine(GameOver(2));//等待2秒后重开

        //存分数
        PlayerPrefs.SetInt("lastlong",UIController.Instance.Length);
        PlayerPrefs.SetInt("lastscore",UIController.Instance.Score);
        //如果没有存过 bestscore ，就存一个0。如果存过就比较 这个村的值 与 当前的分数对比
        if (PlayerPrefs.GetInt("bestscore",0)< UIController.Instance.Score)
        {
            PlayerPrefs.SetInt("bestlong", UIController.Instance.Length);
            PlayerPrefs.SetInt("bestscore", UIController.Instance.Score);
        }
    }



    /// <summary>
    /// 等待一定时间重开
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator GameOver(float time)
    {
        yield return new WaitForSeconds(time);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
