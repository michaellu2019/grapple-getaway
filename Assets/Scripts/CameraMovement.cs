using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        //camera = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, cameraTransform.position.z);
    }
}
