using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingAnimation : MonoBehaviour
{
    public float speed = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0, speed * Time.deltaTime);
    }
}
