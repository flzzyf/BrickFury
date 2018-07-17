using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : Singleton<ShootManager>
{
    public GameObject missilePrefab;
    public float speed = 3;

	void Start () 
	{
		
	}

    void Update()
    {
        if (GameManager.Instance().gaming)
        {
            //游戏进行中
            if (Input.GetMouseButtonDown(0))
            {
                float clickX = Input.mousePosition.x;
                //点击的行号
                int clickIndex = (int)(clickX / (Screen.width / 4));

                SoundManager.Instance().Play("Shoot");

                StartCoroutine(LaunchMissile(clickIndex));
            }
        }
    }

    IEnumerator LaunchMissile(int _index)
    {
        //创建飞弹
        float launchY = GameManager.GetWorldScrrenSize().y / 2 * -1 - 1;
        Vector2 launchPos = new Vector2(BrickManager.Instance().generateX[_index], launchY);
        GameObject missile = Instantiate(missilePrefab, launchPos, Quaternion.identity);
        //发射飞弹
        float targetY = BrickManager.Instance().bottomY - BrickManager.Instance().brickSize.y / 2;
        while(missile.transform.position.y < targetY)
        {
            missile.transform.Translate(speed * Vector2.up * Time.deltaTime);
            targetY += BrickManager.Instance().fallingSpeed * Time.deltaTime;
            yield return null;
        }

        //命中后
        SoundManager.Instance().Play("Boom");


        //命中空缺
        if (BrickManager.Instance().rows.First.Value.types[_index] == 0)
        {
            Debug.Log("命中");

        }
    }
}
