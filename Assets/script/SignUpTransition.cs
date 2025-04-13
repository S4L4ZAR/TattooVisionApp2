using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpControl : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void SignUp(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

}