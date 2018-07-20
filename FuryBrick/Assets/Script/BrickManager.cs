using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : Singleton<BrickManager>
{
    public GameObject brickPrefab;
    public Vector2 brickSize;
    public float fallingSpeed = 0.8f;

    [HideInInspector]
    public float generateY;
    [HideInInspector]
    public float[] generateX;
    float destoryY;

    float generateInterval;
    float generateCD = 0;
    //每行的方块数据
    [HideInInspector]
    public Queue<row> rows = new Queue<row>();
    Queue<GameObject[]> rowBricks = new Queue<GameObject[]>();

	void Start () 
	{
        Init();
    }
	
	void Update () 
	{
        if (!GameManager.Instance().gaming)
            return;

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

    void Init()
    {
        generateY = zyf.GetWorldScreenSize().y / 2 + brickSize.y / 2;

        float cubeInterval = (zyf.GetWorldScreenSize().x - brickSize.x * 4) / 4;
        //左边源点
        float x = zyf.GetWorldScreenSize().x / 2 * -1;
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
        destoryY = zyf.GetWorldScreenSize().y / 2 * -1 - brickSize.y / 2;

        //生成方块时间间隔
        generateInterval = brickSize.y / fallingSpeed;
    }

    GameObject[] brickTemp = new GameObject[4];
    //生成砖块并下落
    IEnumerator GenerateBrick(int _index, int _type)
    {
        float x = generateX[_index];
        Vector2 pos = new Vector2(x, generateY);
        //生成砖块
        GameObject brick = ObjectPoolManager.Instance().SpawnObject(_type , pos, Quaternion.identity);
        brick.transform.localScale = new Vector3(brickSize.x, brickSize.y);

        brickTemp[_index] = brick;

        while(GameManager.Instance().gaming && brick.transform.position.y > destoryY)
        {
            brick.transform.Translate(fallingSpeed * Vector2.down * Time.deltaTime);
            yield return null;
        }
        //清除砖块
        if(GameManager.Instance().gaming)
            brick.SetActive(false);
    }

    //根据数组生成方块行
    void GenerateBrickLine(row _row)
    {
        for (int i = 0; i < GameManager.Instance().rowCount; i++)
        {
            int rowIndex = rowBricks.Count;
            //非空砖块
            if(_row.types[i] > 0)
                StartCoroutine(GenerateBrick(i, _row.types[i]));
            
        }
        //加入方块数列
        rowBricks.Enqueue(brickTemp);

        brickTemp = new GameObject[4];

        //for (int i = 0; i < GameManager.Instance().rowCount; i++)
        //{
        //    brickTemp[i] = null;

        //}
    }

    //随机生成三列
    //0空白，1砖块，2金币砖块，3特殊，4坚固砖块
    void GenerateBrickRandom()
    {
        int spaceIndex = Random.Range(0, 4);
        int[] line = { 1, 1, 1, 1 };

        //随机金币行
        if(zyf.IfItWins(6))
        {
            int index = Random.Range(0, 4);
            line[index] = 2;
        }

        //空行
        line[spaceIndex] = 0;
        //行数据加入链表
        row newRow = new row(line);
        rows.Enqueue(newRow);

        GenerateBrickLine(newRow);
    }

    public void ClearFirstRow()
    {
        for (int i = 0; i < GameManager.Instance().rowCount; i++)
        {
            int type = rows.Peek().types[i];
            if (type > 0)
            {
                GameObject go = rowBricks.Peek()[i];
                go.SetActive(false);

                ParticleManager.Instance().InstantiateParticle("Impact_Brick", go.transform.position);

                //金币块
                if(type == 2)
                {
                    GameManager.Instance().ModifyCoin(1);
                }

            }
        }
        rowBricks.Dequeue();
        rows.Dequeue();

        GameManager.Instance().ClearRow();
    }

    public class row
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
