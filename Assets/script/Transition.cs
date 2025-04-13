using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void Login(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

