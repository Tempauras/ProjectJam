using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Cinemachine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerBehviour : MonoBehaviour
{
    [SerializeField] LayerMask _whatIsGround;
    
    private float xAxis;
    private bool canJump = false;
    private float shakeTimer;

    private Animator _animator;
    private float oldCamPosY;


    public bool explosion = false;

    public int moveSpeed;
    public Rigidbody2D _rigidbody;
    public Camera camera;
    public CinemachineVirtualCamera CinemachineVirtualCamera;
    public GameObject firePoint;
    public GameObject arms;
    public int lifePoints;
    public int maxLifePoints;
    public int jumpHeight;
    public float rateOfFire;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        xAxis = 0;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (xAxis != 0)
        {
            _rigidbody.velocity = new Vector2(xAxis * moveSpeed, _rigidbody.velocity.y);
            _animator.SetFloat("Velocity", Mathf.Abs(xAxis));
        }

        if (xAxis == 0)
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            _animator.SetFloat("Velocity", Mathf.Abs(xAxis));
        }

        canJump = Mathf.Abs(_rigidbody.velocity.y) < 0.01f && _rigidbody.IsTouchingLayers(_whatIsGround);

        shakeTimer -= Time.deltaTime;

        if (shakeTimer<= 0f)
        {
            ShakeCamera(0f,0f);
            camera.transform.position = new Vector3(camera.transform.position.x, oldCamPosY,
            camera.transform.position.z);
        }
        
        Vector2 destination = camera.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 direction = destination - new Vector2(transform.position.x, transform.position.y);
        
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
        
    }

    public void OnMovement(InputValue prmInputValue)
    {
        xAxis = prmInputValue.Get<float>();
    }

    public void OnShoot(InputValue prmValue)
    {
        

        if (prmValue.isPressed)
        {
            oldCamPosY = camera.transform.position.y;
            InvokeRepeating(nameof(Burst), 0f, rateOfFire);
        }
        else
        {
            Debug.Log("cancel");
            if (IsInvoking())
            {
                CancelInvoke();
            }
             ShakeCamera(0f,0f);
             camera.transform.position = new Vector3(camera.transform.position.x, oldCamPosY,
                 camera.transform.position.z);
             camera.transform.rotation = quaternion.identity;
             

        }
    }

    public void Shoot()
    {
        GameObject projectileGameObject =
            GameManager.instance.SpawnFromPool(TypeOfPool.PLAYERBULLET, firePoint.transform);
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
        
        
         if (explosion)
         {
             ShakeCamera(15f, 0.1f);
             explosion = false;
             camera.transform.position = new Vector3(camera.transform.position.x, oldCamPosY,
                 camera.transform.position.z);
             camera.transform.rotation = quaternion.identity;
             
         }
         else
        {
            ShakeCamera(1f, 0.1f);
        }

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