using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBhaviour : MonoBehaviour
{

    public int damage;
    public int force;
    
    public int travelDistance;

    private Rigidbody2D _rigidbody2D;
    
    private Vector2 direction;
    private Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        StartPos = transform.position;

    }

    private void OnEnable()
    {
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.velocity = direction * force;

        if ((transform.position - StartPos).magnitude > travelDistance)
        {
            enabled = false;
        }
        
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        BaseEnemy enemy = col.GetComponent<BaseEnemy>();

        if (enemy)
        {
            enemy.Hit(damage);
            enabled = false;
        }

        if (!col.GetComponent<PlayerBehviour>() && !col.gameObject.CompareTag("BoundingBox") && !col.GetComponent<BulletBhaviour>())
        {
            enabled = false;
        }
    }

    public void SetDirection(Vector2 prmDirection)
    {
        direction = prmDirection;
    }

    private void OnDisable()
    {
        Debug.Log("BulletDisabled");
        GameManager.instance.AddToPool(TypeOfPool.PLAYERBULLET, gameObject);
        gameObject.SetActive(false);
    }
}
