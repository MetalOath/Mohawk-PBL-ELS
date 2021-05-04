using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SplashScreenTimer(3f));
    }
    IEnumerator SplashScreenTimer(float waitTime)
    {
        //Do something before waiting.

        //yield on a new YieldInstruction that waits for X seconds.
        yield return new WaitForSeconds(waitTime);

        //Do something after waiting.
        SceneManager.LoadScene(1);
    }
}
