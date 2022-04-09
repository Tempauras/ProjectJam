using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehviour : MonoBehaviour
{
    
    public int moveSpeed;
    
    private Rigidbody2D _rigidbody;
    private float _xAxis;
    private bool jump = false;

    public Camera camera;
    public Object projectile;
    public int lifePoints;
    public int maxLifePoints;
    public int jumpHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _xAxis = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_xAxis != 0)
        {
            _rigidbody.velocity = new Vector2(_xAxis * moveSpeed, _rigidbody.velocity.y);
        }

        if (_xAxis == 0)
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        }

        if (_rigidbody.velocity.y == 0)
        {
            jump = false;
        }
        
        // if (Input.GetButtonDown("Jump"))
        // {
        //     Debug.Log("Jump");
        //    
        //     _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y + jumpHeight);
        //     Debug.Log(_rigidbody.velocity);
        // }

        
    }

    public void OnMovement(InputValue prmInputValue)
    {
        _xAxis = prmInputValue.Get<float>();
    }

    public void OnShoot()
    {
        GameObject projectileGameObject = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        BulletBhaviour bullet = projectileGameObject.GetComponent<BulletBhaviour>();

        Vector2 destination = camera.ScreenToWorldPoint(Input.mousePosition);
        
        Debug.Log(destination);
        Vector2 direction =  destination - new Vector2(transform.position.x, transform.position.y);
        
        bullet.SetDirection(direction.normalized);
        
        float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        bullet.transform.rotation = Quaternion.Euler(0, 0, lookAngle);
        

    }

    public void OnJump()
    {
        if (!jump)
        {
            Debug.Log("JUMP");
            _rigidbody.AddForce(new Vector2(_rigidbody.velocity.x, jumpHeight), ForceMode2D.Impulse);
            jump = true;
        }
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
