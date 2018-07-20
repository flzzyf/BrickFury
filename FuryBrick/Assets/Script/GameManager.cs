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

    void Start () 
	{
        Init();

        int coin = PlayerPrefs.GetInt("BrickFury_Coin");
        SetCoin(coin);
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
            if(bottomY > zyf.GetWorldScreenSize().y / 2 * -1)
            {
                //降低底线
                bottomY -= BrickManager.Instance().fallingSpeed * Time.deltaTime;

                redLine.transform.position = new Vector2(0, bottomY + BrickManager.Instance().brickSize.y / 11);
                //redLine.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(new Vector2(0, bottomY + BrickManager.Instance().brickSize.y / 11));
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

        bottomY = zyf.GetWorldScreenSize().y / 2;
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

    public void SetCoin(int _amount)
    {
        PlayerPrefs.SetInt("BrickFury_Coin", _amount);
        text_coin.text = _amount.ToString();
    }

    public void ModifyCoin(int _amount)
    {
        SetCoin(PlayerPrefs.GetInt("BrickFury_Coin") + _amount);
    }

}
