using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFeed : MonoBehaviour
{
    private float destroyTime = 4.0f;

    private void OnEnable()
    {
        Destroy(gameObject, destroyTime);
    }
}
