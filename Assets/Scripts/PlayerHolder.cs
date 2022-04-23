using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{

    [Tooltip("Can changable character movement speed")]
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.Instance.canLevelBegin)
        {
            transform.Translate(transform.forward * speed);

        }
    }
}
