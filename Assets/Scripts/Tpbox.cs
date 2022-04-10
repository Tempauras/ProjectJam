using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tpbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                GetComponent<Menu>().MainMenu();
            }
            else
            {
                GetComponent<Menu>().LoadContext2();
            }
                
        }
       
    }
}
