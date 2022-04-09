using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehviour : MonoBehaviour
{
    
    public int moveSpeed;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _xzAxis;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _xzAxis = Vector2.zero;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_xzAxis.magnitude);
        
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
        Debug.Log("XZAXIS start");

        _xzAxis = prmInputValue.Get<Vector2>();

        Debug.Log("XZAXIS end");
        
        
        
    }
}
