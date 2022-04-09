using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehviour : MonoBehaviour
{
    
    public int moveSpeed;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _xzAxis;

    public Camera camera;
    public Object projectile;
    public int lifePoints;
    public int maxLifePoints;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _xzAxis = Vector2.zero;

    }

    // Update is called once per frame
    void Update()
    {
        if (_xzAxis.magnitude != 0)
        {
            _rigidbody.velocity = _xzAxis * moveSpeed;
        }

        if (_xzAxis.magnitude == 0)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        
        
    }

    public void OnMovement(InputValue prmInputValue)
    {
        _xzAxis = prmInputValue.Get<Vector2>();
    }

    public void OnShoot()
    {
        GameObject projectileGameObject = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        BulletBhaviour bullet = projectileGameObject.GetComponent<BulletBhaviour>();

        Vector2 destination = camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction =  destination - new Vector2(transform.position.x, transform.position.y);
        
        bullet.SetDirection(direction.normalized);
        
        float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        bullet.transform.rotation = Quaternion.Euler(0, 0, lookAngle);
        

    }

    public void Hit(int prmDamage)
    {
        lifePoints -= prmDamage;
        if (lifePoints <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("Death");
    }

}
