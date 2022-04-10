using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BaseEnemy : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject arms;
    public Sprite[] enemySprite;
    public Sprite[] enemyArmsSprite;
    [Header("AI Stuff")] public GameObject player;
    public Rigidbody2D rb;
    [Header("Stats")] [SerializeField] private float _lifePoints;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackDelay;
    [SerializeField] private int _range;
    [SerializeField] private int _moveSpeed;
    public bool knockBack = false;
    private float _nextAttack = 0.2f;

    private bool _shooting = false;
    [SerializeField] private Object _bullet;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(0, enemySprite.Length);
        GetComponent<SpriteRenderer>().sprite = enemySprite[random];
        arms.GetComponent<SpriteRenderer>().sprite = enemyArmsSprite[random];
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("SeePlayer", 0f, 0.1f);
    }

    private void OnEnable()
    {
        int random = Random.Range(0, enemySprite.Length);
        GetComponent<SpriteRenderer>().sprite = enemySprite[random];
        arms.GetComponent<SpriteRenderer>().sprite = enemyArmsSprite[random];
    }

    // Update is called once per frame
    void Update()
    {
        if (!knockBack)
        {
            direction = (player.transform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * _moveSpeed, rb.velocity.y);
            if (direction.x <= 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                arms.transform.rotation =
                    Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, -90) * direction);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                arms.transform.rotation =
                    Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * direction);
            }

            if (_shooting)
            {
                if (transform.localPosition.x - player.transform.localPosition.x <= _range &&
                    transform.localPosition.y - player.transform.localPosition.y <= _range &&
                    Time.time > _nextAttack)
                {
                    Attack();
                }
            }
        }
        else
        {
            Invoke(nameof(Reset), 0.2f);
        }
        
        if (transform.position.y <= -5)
        {
            enabled = false;
        }
    }

    public void Reset()
    {
        knockBack = false;
    }

    public void SeePlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, direction, 1000);
        Debug.DrawRay(firePoint.transform.position, direction, Color.blue, 1.0f);
        if (hit.collider == player.GetComponent<BoxCollider2D>())
        {
            ChasePlayer();
        }
    }

    public void ChasePlayer()
    {
        _shooting = true;
    }

    public void Attack()
    {
        GameObject projectileGameObject =
            (GameObject)Instantiate(_bullet, firePoint.transform.position, firePoint.transform.rotation);
        EnemyBulletBehaviour bullet = projectileGameObject.GetComponent<EnemyBulletBehaviour>();

        bullet.bulletSpeed = 10;
        bullet.travelDistance = _range;
        bullet.damage = _damage;

        Vector2 destination = player.transform.position;

        bullet.SetDirection(direction);
        float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, lookAngle);

        _nextAttack = Time.time + _attackDelay;
    }

    public void Hit(int prmDamage)
    {
        _lifePoints -= prmDamage;
        if (_lifePoints <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        this.enabled = false;
    }

    private void OnDisable()
    {
        GameManager.instance.AddToPool(TypeOfPool.ENEMY, gameObject);
        gameObject.SetActive(false);
    }
}