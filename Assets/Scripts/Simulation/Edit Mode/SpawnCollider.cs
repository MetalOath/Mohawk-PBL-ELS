using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    public bool isIntersecting = false;
    [SerializeField] private Material redMat, nullMat;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter(Collider other)
    {
        isIntersecting = true;
        gameObject.GetComponent<Renderer>().material = redMat;
    }
    private void OnTriggerStay(Collider other)
    {
        isIntersecting = true;
        gameObject.GetComponent<Renderer>().material = redMat;
    }
    private void OnTriggerExit(Collider other)
    {
        isIntersecting = false;
        gameObject.GetComponent<Renderer>().material = nullMat;
    }
}
