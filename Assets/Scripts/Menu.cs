using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLvl1()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLvl2()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadContext1()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadContext2()
    {
        SceneManager.LoadScene(3);
    }
}
