using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : Singleton<BrickManager>
{
    public GameObject brickPrefab;
    public Vector2 brickSize;
    public float fallingSpeed = 0.8f;

    float generateY;
    float[] generateX;
    float destoryY;

	void Start () 
	{
        Init();

        int[] line = { 0, 1, 2 };
        GenerateBrick(line);
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
                brickSize.y = GameManager.GetWorldScrrenSize().y / 12;
                Gizmos.DrawCube(new Vector2(generateX[i], generateY - j * (brickSize.y + 0.05f)), brickSize);
            }
        }
    }

    void Init()
    {
        generateY = GameManager.GetWorldScrrenSize().y / 2 + brickSize.y / 2;

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

        //消失高度
        destoryY = GameManager.GetWorldScrrenSize().y / 2 * -1 - brickSize.y / 2;
    }

    IEnumerator GenerateBrick(int _index)
    {
        float x = generateX[_index];
        Vector2 pos = new Vector2(x, generateY);
        //生成砖块
        GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity);
        brick.transform.localScale = new Vector3(brickSize.x, brickSize.y);
        while(brick.transform.position.y > destoryY)
        {
            brick.transform.Translate(fallingSpeed * Vector2.down * Time.deltaTime);
            yield return null;
        }
        //清除砖块
        Destroy(brick);
    }

    void GenerateBrick(int[] _line)
    {
        for (int i = 0; i < _line.Length; i++)
        {
            StartCoroutine(GenerateBrick(_line[i]));
        }
    }
}
