using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
