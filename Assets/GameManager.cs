using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Queue<GameObject> EnemyPool = new Queue<GameObject>();
    private int EnemyPoolSize;
    private Queue<GameObject> EnemyBulletPool = new Queue<GameObject>();
    private int EnemyBulletPoolSize;
    private Queue<GameObject> PlayerBulletPool = new Queue<GameObject>();
    private int PlayerBulletPoolSize;
    private Queue<GameObject> ExplosionPool = new Queue<GameObject>();
    private int ExplosionPoolSize;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitPool(EnemyPool, EnemyPoolSize);
    }

    public void InitPool(Queue<GameObject> pool, int sizeOfPool)
    {
        for (int i = 0; i < sizeOfPool; i++)
        {
            GameObject clone = Instantiate()
        }
    }
}
