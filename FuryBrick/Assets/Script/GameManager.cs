using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public Vector2 cubeSize;
    //public float cubeInterval;

    void Start () 
	{
        Debug.Log(GetWorldScrrenSize());
    }
	
	void Update () 
	{
		
	}

    private void OnDrawGizmos()
    {
        float cubeInterval = (GetWorldScrrenSize().x - cubeSize.x * 4) / 4;
        //左边源点
        float x = GetWorldScrrenSize().x / 2 * -1;
        float y = 0;
        //方块1
        x += cubeInterval / 2;
        for (int i = 0; i < 4; i++)
        {
            x += cubeSize.x / 2;

            Gizmos.DrawCube(new Vector2(x, y), cubeSize);
            x += cubeSize.x / 2;

            x += cubeInterval;

        }

    }

    //获取实际游戏世界屏幕尺寸
    Vector2 GetWorldScrrenSize()
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
