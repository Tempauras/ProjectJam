using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    public int damage;
    public int bulletSpeed;

    public int travelDistance;
    
    private Rigidbody2D _rigidbody2D;
    
    private Vector2 _direction;
    private Vector3 _startPos;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
    }

    private void OnEnable()
    {
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.velocity = _direction * bulletSpeed;

        if ((transform.position - _startPos).magnitude > travelDistance)
        {
            enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerBehviour player = col.GetComponent<PlayerBehviour>();

        if (player)
        {
            player.Hit(bulletSpeed);
            enabled = false;
        }
        
        
        if (!col.GetComponent<EnemyBulletBehaviour>() && !col.gameObject.CompareTag("BoundingBox") && !col.GetComponent<BaseEnemy>())
        {
            enabled = false;
        }
        
    }

    public void SetDirection(Vector2 prmDirection)
    {
        _direction = prmDirection;
    }

    private void OnDisable()
    {
        GameManager.instance.AddToPool(TypeOfPool.ENEMYBULLET, gameObject);
        gameObject.SetActive(false);
    }
}
