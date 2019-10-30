using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public bool ComeToLife
    {
        get;set;
    }

    public bool IsPicked
    {
        get;set;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ComeToLife && !IsPicked)
            ApplyForce();
    }

    void ApplyForce()
    {
        transform.Translate(0, 0, -2f * Time.deltaTime);
    }
}
