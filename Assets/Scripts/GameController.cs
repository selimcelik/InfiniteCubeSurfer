using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Text touchToStartText;

    public bool canLevelBegin = false;

    public GameObject player;

    public List<GameObject> cubes = new List<GameObject>();
    public List<GameObject> destroyObjects = new List<GameObject>();

    [Tooltip("Collected cubes parent")]
    public Transform cubesParent;

    [Tooltip("Neccessary for first touch by player to cube")]
    public float movePlayerOnY = 0;

    [Tooltip("Previous cube")]
    public GameObject prevObj;

    [Tooltip("Can changable cubes space")]
    public float cubesZAxisSpace = 0;

    [Tooltip("Neccessary for creating cubes")]
    private float cubeZAxis = 0;

    [Tooltip("Neccessary for new arrangement after jackpot")]
    private float cubeYAxis = 0;

    [Tooltip("After jackpot adding to destroy list")]
    private bool blue, green, purple, red, yellow = false;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Create Cubes
        CanCreateCube();

        //Event Describing
        EventManager.Instance.onGameStarted += GameStart;
        EventManager.Instance.onCreateCubes += CanCreateCube;
        EventManager.Instance.onCollectCubes += CollectCube;
        EventManager.Instance.onJackpotCubes += JackPot;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canLevelBegin)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }

            
        }
        else
        {
            if (cubes.Count > 0)
            {
                //Previous object describe
                if (prevObj == null)
                {
                    prevObj = cubes[cubes.Count - 1];
                    prevObj.GetComponent<BoxCollider>().isTrigger = true;
                }
            }
            else
            {
                //Animation control
                if (!player.GetComponent<Animator>().GetBool("canRun"))
                {
                    player.transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
                    player.GetComponent<Animator>().SetBool("canSkate", false);
                    player.GetComponent<Animator>().SetBool("canRun", true);
                }
            }
        }



        
    }

    private void GameStart()
    {
        canLevelBegin = true;
        touchToStartText.enabled = false;
    }

    //Create Cubes
    private void CanCreateCube()
    {
        if (!canLevelBegin)
        {
            for (int i = 0; i < 20; i++)
            {
                ObjectPooler.Instance.SpawnForGameObject("cube", new Vector3(Random.Range(-1.5f, 1.5f), 0.32f, cubeZAxis + cubesZAxisSpace), Quaternion.identity, ObjectPooler.Instance.poolObject.transform);
                cubeZAxis += cubesZAxisSpace;
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                ObjectPooler.Instance.SpawnForGameObject("cube", new Vector3(Random.Range(-1.5f, 1.5f), 0.32f, cubeZAxis + cubesZAxisSpace), Quaternion.identity, ObjectPooler.Instance.poolObject.transform);
                cubeZAxis += cubesZAxisSpace;
            }

        }
        
    }

    //Collect cube fonc
    private void CollectCube(GameObject cube)
    {
        //Animation change
        player.GetComponent<Animator>().SetBool("canRun", false);
        player.GetComponent<Animator>().SetBool("canSkate", true);

        
        prevObj.GetComponent<BoxCollider>().isTrigger = false;

        //set cube parent and position
        cube.transform.SetParent(cubesParent);
        Vector3 pos = prevObj.transform.localPosition;
        pos.y += -0.65f;
        cube.transform.localPosition = pos;

        //set player position
        Vector3 characterPos = player.transform.localPosition;
        characterPos.y += 0.65f;
        player.transform.localPosition = characterPos;
        prevObj = cube;

        //some component games :)
        prevObj.gameObject.AddComponent<Rigidbody>();
        prevObj.gameObject.GetComponent<Rigidbody>().useGravity = false;
        prevObj.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        prevObj.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        prevObj.name = prevObj.GetComponent<Cube>().cubeColor.ToString();
        prevObj.tag = "Collector";

        cubes.Add(prevObj.gameObject);

        //Destroy list formation
        switch (destroyObjects.Count)
        {
            case 0:
                destroyObjects.Add(prevObj.gameObject);
                break;
            case 1:
                if (prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[0].GetComponent<Cube>().cubeColor)
                {
                    Instance.destroyObjects.Add(prevObj.gameObject);
                }
                break;
            case 2:
                if (prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[1].GetComponent<Cube>().cubeColor && prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[0].GetComponent<Cube>().cubeColor)
                {
                    Instance.destroyObjects.Add(prevObj.gameObject);
                }
                break;
            case 3:
                if (prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[2].GetComponent<Cube>().cubeColor && prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[1].GetComponent<Cube>().cubeColor && prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[0].GetComponent<Cube>().cubeColor)
                {
                    destroyObjects.Add(prevObj.gameObject);
                }
                break;
            case 4:
                if (prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[3].GetComponent<Cube>().cubeColor && prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[2].GetComponent<Cube>().cubeColor && prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[1].GetComponent<Cube>().cubeColor && prevObj.gameObject.GetComponent<Cube>().cubeColor != destroyObjects[0].GetComponent<Cube>().cubeColor)
                {
                    destroyObjects.Add(prevObj.gameObject);
                }
                break;
        }


        if (destroyObjects.Count >= 5)
        {
            JackPot();
        }
    }

    //Jackpots
    private void JackPot()
    {
        //After formation destroy objects which are in that list
        for (int i = destroyObjects.Count-1; i >= 0; i--)
        {
            GameObject destroyGO = destroyObjects[i];
            cubes.Remove(destroyGO);
            destroyObjects.Remove(destroyGO);
            Destroy(destroyGO);
            blue = false;
            green = false;
            purple = false;
            yellow = false;
            red = false;
        }

        //After jackpot new cubes position arragement and new destroy list formation
        for (int j = cubes.Count - 1; j >= 0; j--)
        {
            switch (cubes[j].name)
            {
                case "blue":
                    if (!blue)
                    {
                        destroyObjects.Add(cubes[j]);
                        blue = true;
                    }
                    break;
                case "green":
                    if (!green)
                    {
                        destroyObjects.Add(cubes[j]);
                        green = true;
                    }
                    break;
                case "purple":
                    if (!purple)
                    {
                        destroyObjects.Add(cubes[j]);
                        purple = true;
                    }
                    break;
                case "red":
                    if (!red)
                    {
                        destroyObjects.Add(cubes[j]);
                        red = true;
                    }
                    break;
                case "yellow":
                    if (!yellow)
                    {
                        destroyObjects.Add(cubes[j]);
                        yellow = true;
                    }
                    break;
            }

            cubes[j].transform.DOMoveY(0.33f+cubeYAxis, .2f);
            cubeYAxis += 0.65f;
            
            if (j == 0)
            {
                cubeYAxis = 0;
                
            }
        }

        //after jackpot player position
        player.transform.DOMoveY(cubesParent.transform.GetChild(0).transform.position.y - 3, .1f);

        

    }

}
