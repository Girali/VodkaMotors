using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
    private static AppController _instance;
    public static AppController Instance
    {
        get 
        { 
            if(_instance == null)
                _instance = FindObjectOfType<AppController>();

            return _instance; 
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1 / 60f;

        DontDestroyOnLoad(gameObject);
    }

    public string mainScene;
    public string loadingScene;


    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;

    public KeyCode interact = KeyCode.E;
    public KeyCode jump = KeyCode.Space;
    public KeyCode sprint = KeyCode.LeftShift;

    public void SelectFR()
    {
        forward = KeyCode.Z;
        left = KeyCode.Q;
        LoadMainScene();
    }

    public void SelectEN()
    {
        LoadMainScene();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
        SceneManager.LoadScene(mainScene);
    }
}
