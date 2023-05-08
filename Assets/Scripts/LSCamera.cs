using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSCamera : MonoBehaviour
{
    public Vector2 minPos, maxPos;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // LateUpdate is called once per frame after Update function
    // Camera is moving after the player moved
    void LateUpdate()
    {
        float xPos = Mathf.Clamp(target.position.x, minPos.x, maxPos.x);
        float yPos = Mathf.Clamp(target.position.y, minPos.y, maxPos.y);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
