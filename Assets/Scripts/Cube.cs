using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    //Cube color types
    public enum CubeColor
    {
        blue,
        green,
        purple,
        red,
        yellow
    }

    public CubeColor cubeColor;

    public Material[] mats;

    MeshRenderer mr;

    private int randomInt = 0;
    // Start is called before the first frame update
    void Start()
    {
        //At the start cube born a random mat 
        randomInt = Random.Range(0, 5);
        mr = GetComponent<MeshRenderer>();
        mr.material = mats[randomInt];

        switch (randomInt)
        {
            case 0:
                cubeColor = CubeColor.blue;
                break;
            case 1:
                cubeColor = CubeColor.green;
                break;
            case 2:
                cubeColor = CubeColor.purple;
                break;
            case 3:
                cubeColor = CubeColor.red;
                break;
            case 4:
                cubeColor = CubeColor.yellow;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Cube" && gameObject.tag =="Collector")
        {
            EventManager.Instance.CollectCubes(other.gameObject);
            EventManager.Instance.CreateCubes();
        }


    }
}
