using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target1;
    [SerializeField] private Transform target2;
    [SerializeField] private Transform target3;

    void LateUpdate()
    {
        Vector3 midpoint = new Vector3();

        midpoint.x = (target1.position.x + target2.position.x + target3.position.x) / 3f;
        midpoint.y = (target1.position.y + target2.position.y + target3.position.y) / 3f;
        midpoint.z = (target1.position.z + target2.position.z + target3.position.z) / 3f;

        transform.position = midpoint;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
