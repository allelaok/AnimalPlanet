using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    
    public Transform otherStair;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Stair");
    }
}
