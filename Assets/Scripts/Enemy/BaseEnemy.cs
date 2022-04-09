using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public GameObject firePoint;
    [Header("AI Stuff")]

    public Transform player;
    private Seeker _seeker;
    private Rigidbody2D _rb;
    private AIDestinationSetter _aiDestinationSetter;
    private Patrol _patrol;
    private AIPath _aiPath;
    [Header("Stats")]
    [SerializeField] private float _lifePoints;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackDelay;
    [SerializeField] private int _range;
    [SerializeField] private int _moveSpeed;
    private float _nextAttack = 0.2f;

    private bool _shooting = false;
    [SerializeField] private Object _bullet;

    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        _aiPath = GetComponent<AIPath>();
        _patrol = GetComponent<Patrol>();
        _aiPath.maxSpeed = _moveSpeed;
        InvokeRepeating("SeePlayer", 0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * lookAngle);
        Debug.Log(_shooting);
        if (_shooting)
        {
           
            if (transform.localPosition.x - player.localPosition.x <= _range && 
                transform.localPosition.y - player.localPosition.y <= _range &&
                Time.time > _nextAttack)
            {
                Attack();
            }
        }
    }

    public void SeePlayer()
    {
        direction = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, direction, 1000);
        Debug.DrawRay(firePoint.transform.position, direction, Color.blue, 1.0f);
        if (hit.collider == player.GetComponent<BoxCollider2D>())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    public void ChasePlayer()
    {
        _aiDestinationSetter.enabled = true;
        _patrol.enabled = false;
        //Debug.Log("Seeing Player");
        _shooting = true;
    }

    public void Patrol()
    {
        _aiDestinationSetter.enabled = false;
        _patrol.enabled = true;
        _shooting = false;

    }

    public void Attack()
    {
        //Debug.Log("Attacking at : " + player.localPosition.x + ", " + player.localPosition.y + " !");
        GameObject projectileGameObject = (GameObject)Instantiate(_bullet, firePoint.transform.position, firePoint.transform.rotation);
        EnemyBulletBehaviour bullet = projectileGameObject.GetComponent<EnemyBulletBehaviour>();
        
        bullet.bulletSpeed = 10;
        bullet.travelDistance = _range;
        bullet.damage = _damage;
        
        Vector2 destination = player.position;

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
        Debug.Log("Enemy Death");
        Destroy(gameObject);
    }
}
