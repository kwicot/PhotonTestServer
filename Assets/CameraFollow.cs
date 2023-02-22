using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    private Transform target;
    void Start()
    {
        
    }

    void Update()
    {
        if (target)
        {
            Vector3 newPosition = target.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.15f);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
