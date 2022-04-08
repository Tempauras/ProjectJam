using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    //Attribute
    [SerializeField] private float _health;

    [SerializeField] private float _damage;

    [SerializeField] private float _moveSpeed;

    [SerializeField] private float _attackSpeed;

    [SerializeField] private float _range;
    
    private NavMeshAgent _agent;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
