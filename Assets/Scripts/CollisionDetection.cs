using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollisionDetection : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Players first touch to Cube
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Cube")
        {
            GetComponent<Animator>().SetBool("canRun", false);
            GetComponent<Animator>().SetBool("canSkate", true);
            GameController.Instance.cubes.Add(other.gameObject);
            other.gameObject.tag = "Collector";
            other.name = other.GetComponent<Cube>().cubeColor.ToString();
            other.gameObject.transform.SetParent(GameController.Instance.cubesParent);
            GameController.Instance.movePlayerOnY += 0.7f;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + GameController.Instance.movePlayerOnY, transform.localPosition.z);
            other.gameObject.transform.localPosition = new Vector3(0, -0.33f, 0);
            other.gameObject.AddComponent<Rigidbody>();
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            GameController.Instance.prevObj = other.gameObject;
            EventManager.Instance.CreateCubes();
            GameController.Instance.destroyObjects.Add(other.gameObject);
            GameController.Instance.movePlayerOnY =0f;
        }
    }
}
