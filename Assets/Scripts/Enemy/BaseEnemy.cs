using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BaseEnemy : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject arms;
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
    public int random;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        // if (SceneManager.GetActiveScene().buildIndex ==4)
        // {
        //     
        // }
        // _animator = GetComponent<Animator>();
        // random = Random.Range(0, enemyArmsSprite.Length);
        // _animator.SetInteger("random", random);
        // arms.GetComponent<SpriteRenderer>().sprite = enemyArmsSprite[random];
        // player = GameObject.FindGameObjectWithTag("Player");
        // rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            random = Random.Range(enemyArmsSprite.Length/2, enemyArmsSprite.Length);
        }
        else
        {
            random = Random.Range(0, enemyArmsSprite.Length/2);
        }
        
        
        _animator.SetInteger("random", random);
        arms.GetComponent<SpriteRenderer>().sprite = enemyArmsSprite[random];
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!knockBack && player)
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
            if (transform.localPosition.x - player.transform.localPosition.x <= _range &&
                transform.localPosition.y - player.transform.localPosition.y <= _range &&
                Time.time > _nextAttack)
                {
                    Attack();
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

    public void Attack()
    {
        GameObject projectileGameObject = GameManager.instance.SpawnFromPool(TypeOfPool.ENEMYBULLET, firePoint.transform);
        projectileGameObject.SetActive(true);
        EnemyBulletBehaviour bullet = projectileGameObject.GetComponent<EnemyBulletBehaviour>();
        bullet.enabled = true;
       
        bullet.bulletSpeed = 10;
        bullet.travelDistance = _range;
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            bullet.damage = _damage * 2;
        }
        else
        {
            bullet.damage = _damage;
        }
        

        Vector2 destination = player.transform.position;

        bullet.SetDirection(direction);
        float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, lookAngle);

        _nextAttack = Time.time + _attackDelay;
    }

    public void Hit(int prmDamage)
    {
        _spriteRenderer.color = Color.red;
        arms.GetComponent<SpriteRenderer>().color = Color.red;
        
        _lifePoints -= prmDamage;
        if (_lifePoints <= 0)
        {
            Death();
        }
        Invoke(nameof(changeColor), 1.5f);
    }

    public void changeColor()
    {
        _spriteRenderer.color = Color.white;
        arms.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Death()
    {
        if ((int)Random.Range(1, 100) <= 10)
        {
            player.GetComponent<PlayerBehviour>().explosion = true;
            GameObject clone = GameManager.instance.SpawnFromPool(TypeOfPool.EXPLOSION, transform);
            clone.SetActive(true);
            Explosion ekusuplosion = clone.GetComponent<Explosion>();
            ekusuplosion.enabled = true;
        }
        this.enabled = false;
    }
    private void OnDisable()
    {
        GameManager.instance.AddToPool(TypeOfPool.ENEMY, gameObject);
        gameObject.SetActive(false);
    }
}