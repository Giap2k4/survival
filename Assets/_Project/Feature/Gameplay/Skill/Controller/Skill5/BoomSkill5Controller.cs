using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomSkill5Controller : MonoBehaviour
{
    private ParticleSystem[] particles;

    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckFinish());
    }

    private IEnumerator CheckFinish()
    {
        while (true)
        {
            bool alive = false;

            foreach (var ps in particles)
            {
                if (ps.IsAlive(true))
                {
                    alive = true;
                    break;
                }
            }

            if (!alive)
                break;

            yield return null;
        }

        gameObject.SetActive(false); // trả về pool
        PoolingManager.AddBoomSkill5Pool(gameObject);
    }
}
