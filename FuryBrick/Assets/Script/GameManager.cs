using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public bool gaming = false;
    bool readyToStart;

    public GameObject text_hint;

    public int rowCount = 4;

    public GameObject redLine;

    public Text text_score;
    public Text text_coin;
    public Text text_combo;

    public GameObject panel_gameover;

    int score;
    int coin;
    int combo;

    [HideInInspector]
    public float bottomY;

    [HideInInspector]
    public Vector2 worldScreenSize;

    void Start () 
	{
        Init();

        worldScreenSize = GetWorldScreenSize();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SoundManager.Instance().Play("Shoot");
        }

        if (!gaming)
        {
            //游戏没开始
            if (Input.GetMouseButtonDown(0) && readyToStart)
            {
                readyToStart = false;

                GameStart();
            }
        }
        else
        {
            //游戏进行中
            if(bottomY > worldScreenSize.y / 2 * -1)
            {
                //降低底线
                bottomY -= BrickManager.Instance().fallingSpeed * Time.deltaTime;

                redLine.transform.position = new Vector2(0, bottomY + BrickManager.Instance().brickSize.y / 11);
            }
            else
            {
                //失败
                GameOver();
            }
            
        }
    }

    void Init()
    {
        score = 0;
        text_score.text = "0";

        combo = 0;
        text_combo.text = "0";

        readyToStart = true;

        bottomY = worldScreenSize.y / 2;
    }

    void GameStart()
    {
        gaming = true;

        text_hint.SetActive(false);

        SoundManager.Instance().Play("BGM");

    }

    void GameOver()
    {
        gaming = false;

        SoundManager.Instance().StopPlay("BGM");

        panel_gameover.SetActive(true);
    }

    public void GameReset()
    {
        Init();

        text_hint.SetActive(true);

        panel_gameover.SetActive(false);

        //移除现有砖块

    }

    public void ClearRow()
    {
        bottomY += BrickManager.Instance().brickSize.y;

        score++;
        text_score.text = score.ToString();
    }

    //获取实际游戏世界屏幕尺寸
    public static Vector2 GetWorldScreenSize()
    {
        float leftBorder;
        float rightBorder;
        float topBorder;
        float downBorder;
        //the up right corner
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        float width = rightBorder - leftBorder;
        float height = topBorder - downBorder;

        return new Vector2(width, height);
    }
}
