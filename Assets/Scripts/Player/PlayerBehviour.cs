using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerBehviour : MonoBehaviour
{
    [SerializeField] LayerMask _whatIsGround;
    
    private float xAxis;
    private bool canJump = false;
    private float shakeTimer;


    public int moveSpeed;
    public Rigidbody2D _rigidbody;
    public Camera camera;
    public CinemachineVirtualCamera CinemachineVirtualCamera;
    public int lifePoints;
    public int maxLifePoints;
    public int jumpHeight;
    public float rateOfFire;
    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        xAxis = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (xAxis != 0)
        {
            _rigidbody.velocity = new Vector2(xAxis * moveSpeed, _rigidbody.velocity.y);
        }

        if (xAxis == 0)
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        }

        canJump = Mathf.Abs(_rigidbody.velocity.y) < 0.01f && _rigidbody.IsTouchingLayers(_whatIsGround);

        shakeTimer -= Time.deltaTime;

        if (shakeTimer<= 0f)
        {
            ShakeCamera(0f,0f);
        }
    }

    public void OnMovement(InputValue prmInputValue)
    {
        xAxis = prmInputValue.Get<float>();
    }

    public void OnShoot(InputValue prmValue)
    {
        GameObject projectileGameObject = GameManager.instance.SpawnFromPool(TypeOfPool.PLAYERBULLET, transform);

        if (prmValue.isPressed)
        {
            InvokeRepeating(nameof(Burst), 0f, rateOfFire);
        }
        else
        {
            Debug.Log("cancel");
            if (IsInvoking())
            {
                CancelInvoke();
            }
            
        }
    }

    public void Shoot()
    {
        GameObject projectileGameObject =
            GameManager.instance.SpawnFromPool(TypeOfPool.PLAYERBULLET, transform);
        projectileGameObject.SetActive(true);
        BulletBhaviour bullet = projectileGameObject.GetComponent<BulletBhaviour>();
        bullet.enabled = true;

        Vector2 destination = camera.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 direction = destination - new Vector2(transform.position.x, transform.position.y);

        direction += new Vector2(Random.Range(-0.7f, 0.7f), Random.Range(-0.7f, 0.7f));
        
        bullet.SetDirection(direction.normalized);

        float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bullet.transform.rotation = Quaternion.Euler(0, 0, lookAngle);
    }

    void ShakeCamera(float prmIntensity, float time)
    {
        CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
            prmIntensity;

        shakeTimer = time;
    }

    void Burst()
    {
        for (int i = 0; i < 3; i++)
        {
            Shoot();
            
            
        }
        ShakeCamera(5f, 0.1f);
    }
    
    public void OnJump()
    {
        if (canJump)
        {
            Debug.Log("JUMP");
            _rigidbody.AddForce(new Vector2(_rigidbody.velocity.x, jumpHeight), ForceMode2D.Impulse);
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