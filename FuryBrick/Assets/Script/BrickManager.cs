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

    float generateInterval;
    float generateCD = 0;
    //一行中空缺的位置序号
    LinkedList<row> rowSpace = new LinkedList<row>();

	void Start () 
	{
        Init();

    }
	
	void Update () 
	{
		if(generateCD <= 0)
        {
            generateCD = generateInterval;

            GenerateBrickRandom();
        }
        else
        {
            generateCD -= Time.deltaTime;
        }
	}

    //显示方块所占空间
    //private void OnDrawGizmos()
    //{
    //    Init();
    //    for (int j = 0; j < 12; j++)
    //    {
    //        for (int i = 0; i < 4; i++)
    //        {
    //            brickSize.y = GameManager.GetWorldScrrenSize().y / 12;
    //            Gizmos.DrawCube(new Vector2(generateX[i], generateY - j * (brickSize.y + 0.05f)), brickSize);
    //        }
    //    }
    //}

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

        //生成方块时间间隔
        generateInterval = brickSize.y / fallingSpeed;
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

    //根据数组生成方块行
    void GenerateBrickLine(row _row)
    {
        for (int i = 0; i < _row.types.Length; i++)
        {
            if(_row.types[i] == 1)
                StartCoroutine(GenerateBrick(i));
        }
    }

    //随机生成三列
    //0空白，1砖块，2金币砖块，3特殊，4坚固砖块
    void GenerateBrickRandom()
    {
        int spaceIndex = Random.Range(0, 4);
        int[] line = { 1, 1, 1, 1 };
        line[spaceIndex] = 0;
        //行数据加入链表
        row newRow = new row(line);
        rowSpace.AddLast(newRow);

        GenerateBrickLine(newRow);
    }

    class row
    {
        public int[] types = new int[4];

        public row(int _type1, int _type2, int _type3, int _type4)
        {
            types[0] = _type1;
            types[1] = _type2;
            types[2] = _type3;
            types[3] = _type4;
        }

        public row(int[] _types)
        {
            types = _types;
        }
    }
}
