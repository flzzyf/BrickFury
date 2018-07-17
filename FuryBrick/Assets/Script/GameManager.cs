using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public bool gaming = false;

    public GameObject text_hint;

    void Start () 
	{
        Debug.Log(GetWorldScrrenSize());
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
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
        }
        else
        {
            //游戏进行中
            
        }
    }

    void GameStart()
    {
        gaming = true;

        text_hint.SetActive(false);

        SoundManager.Instance().Play("BGM");

    }

    void GameOver()
    {

    }

    void GameReset()
    {
        text_hint.SetActive(true);

    }

    //获取实际游戏世界屏幕尺寸
    public static Vector2 GetWorldScrrenSize()
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
