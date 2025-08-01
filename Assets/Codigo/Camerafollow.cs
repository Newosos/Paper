using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//platano

public class Camerafollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float timeoffset;
    [SerializeField] Vector3 offsetPos;
    [SerializeField] Vector3 boundsMin;
    [SerializeField] Vector3 boundsMax;

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = player.position;

            targetPos.x += offsetPos.x;
            targetPos.y += offsetPos.y;
            //targetPos.z += offsetPos.z;
            targetPos.z = transform.position.z;

          targetPos.x = Mathf.Clamp(targetPos.x, boundsMin.x, boundsMax.x);
         targetPos.y = Mathf.Clamp(targetPos.y, boundsMin.y, boundsMax.y);

            // float tx = 1f - Mathf.Lerp(startPos.x, targetPos.x, timeoffset);
            //float ty = 1f - Mathf.Lerp(startPos.y, targetPos.y, timeoffset);

            float t = 1f - Mathf.Pow(1f - timeoffset, Time.deltaTime * 30);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            


        }
    }
}
