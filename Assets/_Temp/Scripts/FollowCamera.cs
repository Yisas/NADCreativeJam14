using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    public float verticalDamping = 1;
    public float horizonalDamping = 1;
    Vector3 offset;

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        float currentAngley = transform.eulerAngles.y;
        float desiredAngley = target.transform.eulerAngles.y;
        float currentAnglex = transform.eulerAngles.x;
        float desiredAnglex = target.transform.eulerAngles.x;

        float angley = Mathf.LerpAngle(currentAngley, desiredAngley, Time.deltaTime * verticalDamping);
        //float anglex = Mathf.LerpAngle(currentAnglex, desiredAnglex, Time.deltaTime * horizonalDamping);
         
        Quaternion rotation = Quaternion.Euler(0, angley, 0);
        transform.position = target.transform.position - (rotation * offset);

        transform.LookAt(target.transform);
    }
}