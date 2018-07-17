using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParticleManager : Singleton<ParticleManager>
{
    public Particle[] particles;

    GameObject parent_particle;

    void Start () 
	{
        parent_particle = new GameObject("Parent_Particle");
    }

    //生成粒子
    public void InstantiateParticle(string _name, Vector2 _pos)
    {
        //寻找粒子
        Particle particle = Array.Find(particles, sound => sound.name == _name);
        if (particle == null)
        {
            Debug.LogWarning("Particle: " + _name + " not found!");
            return;
        }

        GameObject go = Instantiate(particle.prefab, _pos, Quaternion.identity, parent_particle.transform);
        float duration = particle.prefab.GetComponent<ParticleSystem>().main.duration;
        Destroy(go, duration);
    }

    [System.Serializable]
	public class Particle
    {
        public string name;
        public GameObject prefab;

    }
}
