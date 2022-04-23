using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    [SerializeField] Vector3 movementTransform;
    PlayerInputController playerInputController;

    Vector2 readingValue;
    Vector3 movementValue;
    private bool onClick = false;
    [Tooltip("Can changable mouse or touch sensitivity")]
    [SerializeField] float speed;


    Animator animator;

    private void Awake()
    {
        //Input system describing
        readingValue = Vector2.zero;
        playerInputController = new PlayerInputController();


        playerInputController.CharacterControls.Move.started += movementInput;
        playerInputController.CharacterControls.Move.performed += movementInput;
        playerInputController.CharacterControls.Move.canceled += movementInput;
    }

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        if (GameController.Instance.canLevelBegin)
        {
            if (Input.GetMouseButtonDown(0))
            {
                onClick = true;
            }
            if (Input.GetMouseButton(0))
            {
                //Move by touch
                transform.Translate(-movementValue.x * speed, 0, 0);
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -1.62f, 1.63f), transform.localPosition.y, transform.localPosition.z);
            }
            if (Input.GetMouseButtonUp(0))
            {
                onClick = false;
                readingValue = Vector2.zero;
            }
        }

        animator.SetBool("canRun", GameController.Instance.canLevelBegin);

    }

    //Input system describing
    void movementInput(InputAction.CallbackContext context)
    {
        if (onClick)
        {
            readingValue = context.ReadValue<Vector2>();

            movementValue.x = readingValue.x;

        }


    }

    private void OnEnable()
    {
        playerInputController.Enable();
    }
    private void OnDisable()
    {
        playerInputController.Disable();
    }
}
