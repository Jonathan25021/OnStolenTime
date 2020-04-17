using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector2 viewportSize;
    Camera cam;
    public float viewportFactor;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    public float followDuration;
    public float maxFollowSpeed;

    private Transform cursor;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
    /*
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            ResetCamera();
        }
        else
        {
            FollowCursor();
        }
    }

    private void ResetCamera()
    {
        Vector2 distance = player.position - transform.position;
        Vector2 tempViewportSize = new Vector2(.1f, .1f);

        if (Mathf.Abs(distance.x) > tempViewportSize.x / 2)
        {
            targetPosition.x = player.position.x - (tempViewportSize.x / 2 * Mathf.Sign(distance.x));
        }
        if (Mathf.Abs(distance.y) > tempViewportSize.y / 2)
        {
            targetPosition.y = player.position.y - (tempViewportSize.y / 2 * Mathf.Sign(distance.y));
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition - new Vector3(0, 0, 10),
            ref currentVelocity, followDuration / 2, maxFollowSpeed * 4);
    }

    private void FollowCursor()
    {
        viewportSize = (cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))
            - cam.ScreenToWorldPoint(Vector2.zero)) * viewportFactor;

        Vector2 distance = cursor.position - transform.position;
        if (Mathf.Abs(distance.x) > viewportSize.x / 2)
        {
            targetPosition.x = cursor.position.x - (viewportSize.x / 2 * Mathf.Sign(distance.x));
        }
        if (Mathf.Abs(distance.y) > viewportSize.y / 2)
        {
            targetPosition.y = cursor.position.y - (viewportSize.y / 2 * Mathf.Sign(distance.y));
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition - new Vector3(0, 0, 10),
            ref currentVelocity, followDuration, maxFollowSpeed);
    }
    */
}
