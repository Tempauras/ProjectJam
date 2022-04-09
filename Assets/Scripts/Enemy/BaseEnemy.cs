using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    #region AI stuff
    public Transform target;
    private Seeker _seeker;
    private Rigidbody2D _rb;
    #endregion
    [SerializeField] private Object _bullet;
    [SerializeField] private float _lifePoints;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _range;


    private float _nextAttack = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x - target.localPosition.x <= _range && 
            transform.localPosition.y - target.localPosition.y <= _range &&
            Time.time > _nextAttack)
        {
            Attack();
        }
    }

    public void Attack()
    {
        Debug.Log("Attacking at : " + target.localPosition.x + ", " + target.localPosition.y + " !");
        Instantiate(_bullet, transform.position, transform.rotation, transform);
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
    }
}
