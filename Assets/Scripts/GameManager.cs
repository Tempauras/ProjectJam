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
    
    public Transform EnemyParentGO;
    public Transform EnemyBulletParentGO;
    public Transform PlayerBulletParentGO;
    public Transform ExplosionParentGO;

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
        PlayerBulletPoolSize = 200;
        ExplosionPoolSize = 50;
        InitPool(EnemyPool, EnemyPoolSize, EnemyGO, EnemyParentGO);
        InitPool(EnemyBulletPool, EnemyBulletPoolSize, EnemyBulletGO, EnemyBulletParentGO);
        InitPool(PlayerBulletPool, PlayerBulletPoolSize, PlayerBulletGO, PlayerBulletParentGO);
        InitPool(ExplosionPool, ExplosionPoolSize, ExplosionGO, ExplosionParentGO);
        Debug.Log(PlayerBulletPool.Count);
    }

    private void InitPool(Queue<GameObject> pool, int sizeOfPool, GameObject go, Transform transformParentGo)
    {
        for (int i = 0; i < sizeOfPool; i++)
        {
            GameObject clone = Instantiate(go, new Vector3(0, -150, 0), quaternion.identity, transformParentGo);
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

    public GameObject SpawnFromPool(TypeOfPool typeOfPool, Transform newTransform)
    {
        GameObject obj = null;
        switch (typeOfPool)
        {
            case TypeOfPool.ENEMY :
                obj = EnemyPool.Dequeue();
                break;
            case TypeOfPool.ENEMYBULLET :
                obj = EnemyBulletPool.Dequeue();
                break;
            case TypeOfPool.PLAYERBULLET :
                obj = PlayerBulletPool.Dequeue();
                break;
            case TypeOfPool.EXPLOSION :
                obj = ExplosionPool.Dequeue();
                break;
        }
        obj.transform.position = newTransform.position;
        obj.transform.rotation = newTransform.rotation;
        return obj;
    }
}
