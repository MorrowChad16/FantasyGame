using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //character move speed, editable in Unity
    [SerializeField] private float moveSpeed = 10f;
    //Links character to the movement ability
    private CharacterController characterController;
    //
    [SerializeField] private LayerMask layerMask;
    //
    private Vector3 currentLookTarget = Vector3.zero;
    [SerializeField] private float cameraRotationSpeed = 10f;
    private Animator anim;
    private BoxCollider[] swordColliders;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        swordColliders = GetComponentsInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.GameOver) {
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            characterController.SimpleMove(moveDirection * moveSpeed);

            //if our moveDirection is zero, we aren't moving so use idle animation, otherwise use the walking animation
            if (moveDirection == Vector3.zero)
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
            }

            //left mouse button
            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("Double Chop");
            }

            //right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                anim.Play("Spin Attack");
            }
        }
    }

    //Use FixedUpdate for physics objects
    private void FixedUpdate()
    {

        if (!GameManager.instance.GameOver) {
            //point where raycast hits the layer
            RaycastHit hit;
            //Move the ray to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //See the ray for debugging purposes
            Debug.DrawRay(ray.origin, ray.direction * 500, Color.red);

            if (Physics.Raycast(ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.point != currentLookTarget)
                {
                    currentLookTarget = hit.point;
                }

                //Set the new movement
                Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * cameraRotationSpeed);
            }
        }
    }

    public void BeginAttack()
    {
        foreach (var weapon in swordColliders)
        {
            weapon.enabled = true;
        }
    }

    public void EndAttack()
    {
        foreach (var weapon in swordColliders)
        {
            weapon.enabled = false;
        }
    }
}
