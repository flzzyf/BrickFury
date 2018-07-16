using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : Singleton<BrickManager>
{
    public GameObject brickPrefab;
    public Vector2 brickSize;

    float generateY;
    float[] generateX;

	void Start () 
	{
        Init();
    }
	
	void Update () 
	{
		
	}

    private void OnDrawGizmos()
    {
        Init();
        for (int j = 0; j < 12; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawCube(new Vector2(generateX[i], generateY - j * brickSize.y / 2), brickSize);
            }
        }
        

    }

    void Init()
    {
        generateY = GameManager.GetWorldScrrenSize().y / 2 + 1;

        float cubeInterval = (GameManager.GetWorldScrrenSize().x - brickSize.x * 4) / 4;
        //左边源点
        float x = GameManager.GetWorldScrrenSize().x / 2 * -1;
        //方块1
        x += cubeInterval / 2;
        generateX = new float[4];
        for (int i = 0; i < 4; i++)
        {
            x += brickSize.x / 2;

            generateX[i] = x;
            x += brickSize.x / 2;

            x += cubeInterval;
        }
    }
}
