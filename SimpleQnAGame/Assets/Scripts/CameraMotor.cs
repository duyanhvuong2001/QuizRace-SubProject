using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 0.15f;
    public float boundY = 0.05f;

    private void Start()
    {
       
    }

    private void LateUpdate()//Called after Update
    {
        Vector3 delta = Vector3.zero; //difference between this frame and next frame

        float deltaX = lookAt.position.x - transform.position.x; //transform is the center of the camera

        //Check X axis bound
        if (deltaX > boundX || deltaX < -boundX)
        {
            if (transform.position.x < lookAt.position.x)//if the focus x of the camera is smaller than that of the lookAt
            {
                delta.x = deltaX - boundX;
            }
            else
            {
                delta.x = deltaX + boundX;
            }
        }


        float deltaY = lookAt.position.y - transform.position.y; //transform is the center of the camera

        //Check Y axis bound
        if (deltaY > boundY || deltaY < -boundY)
        {
            if (transform.position.y < lookAt.position.y)//if the focus y of the camera is smaller than that of the lookAt
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }

}
