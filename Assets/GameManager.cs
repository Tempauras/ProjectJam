using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum TypeOfPool
{
    ENEMY,
    ENEMYBULLET,
    PLAYERBULLET,
    EXPLOSION
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject EnemyGO;
    public GameObject EnemyBulletGO;
    public GameObject PlayerBulletGO;
    public GameObject ExplosionGO;

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
        EnemyPoolSize = 200;
        EnemyBulletPoolSize = 500;
        PlayerBulletPoolSize = 50;
        ExplosionPoolSize = 50;
        Debug.Log("On start");
        InitPool(EnemyPool, EnemyPoolSize);
        SpawnFromPool(TypeOfPool.ENEMY, transform);
    }

    private void InitPool(Queue<GameObject> pool, int sizeOfPool)
    {
        for (int i = 0; i < sizeOfPool; i++)
        {
            Debug.Log(i);
            GameObject clone = Instantiate(EnemyGO, new Vector3(0, -150, 0), quaternion.identity);
            clone.SetActive(false);
        }
    }

    public void AddToPool(TypeOfPool typeOfPool, GameObject obj)
    {
        switch (typeOfPool)
        {
            case TypeOfPool.ENEMY :
                EnemyPool.Enqueue(obj);
                break;
            case TypeOfPool.ENEMYBULLET :
                EnemyBulletPool.Enqueue(obj);
                break;
            case TypeOfPool.PLAYERBULLET :
                PlayerBulletPool.Enqueue(obj);
                break;
            case TypeOfPool.EXPLOSION :
                ExplosionPool.Enqueue(obj);
                break;
        }
    }

    public void SpawnFromPool(TypeOfPool typeOfPool, Transform transform)
    {
        switch (typeOfPool)
        {
            case TypeOfPool.ENEMY :
                GameObject obj = EnemyPool.Dequeue();
                obj.transform.position = transform.position;
                obj.SetActive(true);
                obj.GetComponent<BaseEnemy>().enabled = true;
                break;
            case TypeOfPool.ENEMYBULLET :
                break;
            case TypeOfPool.PLAYERBULLET :
                break;
            case TypeOfPool.EXPLOSION :
                break;
        }
    }
}
