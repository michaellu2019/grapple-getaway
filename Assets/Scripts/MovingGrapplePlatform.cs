using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingGrapplePlatform : MonoBehaviour
{
    public BoxCollider2D box;
    public CircleCollider2D player;
    public float range;
    public float speed;
    public bool startLeft = true;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        if (startLeft) {
            startPos = box.transform.position;
            endPos = new Vector3(box.transform.position.x + range, box.transform.position.y, box.transform.position.z);
            movingLeft = false;
        } else {
            startPos = new Vector3(box.transform.position.x - range, box.transform.position.y, box.transform.position.z);
            endPos = box.transform.position;
            movingLeft = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(movingLeft) {
            box.transform.position = Vector3.MoveTowards(box.transform.position, startPos, speed * Time.deltaTime);
        } else {
            box.transform.position = Vector3.MoveTowards(box.transform.position, endPos, speed * Time.deltaTime);
        }

        if(box.transform.position.x >= endPos.x) {
            movingLeft = true;
        } else if(box.transform.position.x <= startPos.x) {
            movingLeft = false;
        }
    }
}
