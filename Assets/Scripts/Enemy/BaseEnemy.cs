using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public GameObject firePoint;
    #region AI stuff
    public Transform target;
    private Seeker _seeker;
    private Rigidbody2D _rb;
    private AIPath _aiPath;
    #endregion
    [SerializeField] private Object _bullet;
    [SerializeField] private float _lifePoints;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _range;
    private float _nextAttack = 0.1f;

    private Vector2 destination;
    
    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _aiPath = GetComponent<AIPath>();
        
        destination = target.position;
        float lookAngle = Mathf.Atan2(destination.y, destination.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, lookAngle);
    }

    // Update is called once per frame
    void Update()
    {
        destination = target.position;
        float lookAngle = Mathf.Atan2(destination.y, destination.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0, 0, lookAngle);
        
        if (transform.localPosition.x - target.localPosition.x <= _range && 
            transform.localPosition.y - target.localPosition.y <= _range &&
            Time.time > _nextAttack)
        {
            Attack();
        }
        
       // transform.LookAt(target, transform.right);
    }

    public void Attack()
    {
        Debug.Log("Attacking at : " + target.localPosition.x + ", " + target.localPosition.y + " !");
        GameObject projectileGameObject = (GameObject)Instantiate(_bullet, transform.position, transform.rotation);
        BulletBhaviour bullet = projectileGameObject.GetComponent<BulletBhaviour>();
        
        Vector2 direction =  destination - new Vector2(transform.position.x, transform.position.y);
        bullet.SetDirection(direction.normalized);
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
        //Destroy(gameObject);
    }
}
