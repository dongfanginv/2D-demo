using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//平台执行自动运动的
public class AirPlat : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 oringipoint;
    
    public Vector3 targetpoint;
    [HideInInspector]
    float speed=4.0f;
    void Start()
    {
        oringipoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x == targetpoint.x)
        {
            targetpoint = oringipoint;
            oringipoint = transform.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetpoint, speed * Time.deltaTime);
    }
}
