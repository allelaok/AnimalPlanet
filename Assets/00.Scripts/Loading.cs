using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField]
    float speed = 1;
    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
    }
    void Update()
    {
        transform.Rotate(0, 0, -speed);
    }
}
