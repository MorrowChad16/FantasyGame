using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollow : MonoBehaviour
{
    //target == hero
    [SerializeField] GameObject target;
    //delay between character movement and camera following
    [SerializeField] float smoothing = 5f;

    Vector3 offset;

    private void Awake()
    {
        //ensures we have a target to follow on the start up of the game
        Assert.IsNotNull(target);
    }

    // Start is called before the first frame update
    void Start()
    {   
        //camera position - hero position
        offset = transform.position - target.transform.position;        
    }

    // Update is called once per frame
    void Update()
    {
        //Tells the camera where to go to follow the character
        Vector3 targetCamPos = target.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, smoothing * Time.deltaTime);
    }
}
