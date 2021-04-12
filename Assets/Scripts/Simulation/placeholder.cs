using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeholder : MonoBehaviour
{
    [SerializeField] private string goTag;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerStay(Collider other)
    {
        other.gameObject.transform.parent.gameObject.tag = goTag;
    }
}
