using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void QuitApp()
    {
        Application.Quit();
    }
    public void NewWorkspace()
    {
        SceneManager.LoadScene("_Main_Scene");
    }
    public void ExampleWorkspace()
    {
        SceneManager.LoadScene("_Example_Scene");
    }
}
