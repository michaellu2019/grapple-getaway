using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LavaMovement : MonoBehaviour
{
    public BoxCollider2D box;
    public CircleCollider2D player;
    private Vector3 startPosition = Vector3.zero;
    float offset = 1;
    float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 boxPos = new Vector3(startPosition.x, box.transform.position.y + offset, box.transform.position.z);
        box.transform.position = Vector3.MoveTowards(box.transform.position, boxPos, speed * Time.deltaTime);

        if (box.IsTouching(player)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
