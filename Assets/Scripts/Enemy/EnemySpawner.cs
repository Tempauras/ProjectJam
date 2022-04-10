using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float delayToSpawn;

    [SerializeField] private int mobToSpawnsPerSeconds;

    [SerializeField] private float radiusForSpawn;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnMob", delayToSpawn, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnMob()
    {
        Debug.Log("SPAWN");
        for (int i = 0; i < mobToSpawnsPerSeconds; i++)
        {
            GameObject clone = GameManager.instance.SpawnFromPool(TypeOfPool.ENEMY, transform);
            clone.transform.position = transform.position;
            clone.SetActive(true);
            clone.GetComponent<BaseEnemy>().enabled = true;
        }
    }
}
