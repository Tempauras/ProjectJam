using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float delayToSpawn;

    [SerializeField] private int mobToSpawnAtEachInterval;

    [SerializeField] private float radius;

    [SerializeField] private float interval;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnMob", delayToSpawn, interval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnMob()
    {
        Debug.Log("SPAWN");
        for (int i = 0; i < mobToSpawnAtEachInterval; i++)
        {
            GameObject clone = GameManager.instance.SpawnFromPool(TypeOfPool.ENEMY, transform);
            clone.transform.position =
                new Vector2(transform.position.x + Random.Range(-radius, radius), transform.position.y);
            clone.SetActive(true);
            clone.GetComponent<BaseEnemy>().enabled = true;
        }
    }
}
