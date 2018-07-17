using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : Singleton<ShootManager>
{
    public GameObject missilePrefab;
    public float speed = 3;

    public GameObject impact_brick;
    public GameObject impact_missile;
    public GameObject impact_coin;

    GameObject parent_particle;

    void Start()
    {
        parent_particle = new GameObject("Parent_Particle");
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

                Shoot(clickIndex);
            }
        }
    }

    void Shoot(int _index)
    {
        SoundManager.Instance().Play("Shoot");

        StartCoroutine(IEShoot(_index));
    }

    IEnumerator IEShoot(int _index)
    {
        //创建飞弹
        float launchY = GameManager.GetWorldScrrenSize().y / 2 * -1 - 1;
        Vector2 launchPos = new Vector2(BrickManager.Instance().generateX[_index], launchY);

        GameObject missile = ObjectPoolManager.Instance().SpawnObject("Missile", launchPos, Quaternion.identity);
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

        InstantiateParticle(impact_coin, missile.transform.position);
        //回收
        missile.SetActive(false);


        //命中空缺
        if (BrickManager.Instance().rows.First.Value.types[_index] == 0)
        {
            Debug.Log("命中");

            InstantiateParticle(impact_brick, missile.transform.position);
        }
    }

    void InstantiateParticle(GameObject _particle, Vector2 _pos)
    {
        GameObject particle = Instantiate(_particle, _pos, Quaternion.identity, parent_particle.transform);
        float duration = particle.GetComponent<ParticleSystem>().main.duration;
        Destroy(particle, duration);
    }
}
