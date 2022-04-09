using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Parallax : MonoBehaviour
{
    private float _xAxis;
    private PlayerBehviour _playerBehviour;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerBehviour = GameObject.FindWithTag("Player").GetComponent<PlayerBehviour>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-_playerBehviour._rigidbody.velocity.x/10000, 0, 0));
        Debug.Log(_playerBehviour._rigidbody.velocity.y);
    }

    
}
