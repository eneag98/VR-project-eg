using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public BoxCollider parentCollider;
    public Vector3 normalDirection;

    Material _material;
    BoxCollider _boxCollider;
    Color _color;
    float _min;
    float _max;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponentInParent<Renderer>().material;
        _color = _material.color;
        _boxCollider = GetComponent<BoxCollider>();
        _min = 0f;
        _max = _boxCollider.size.y;
    }

    void OnTriggerExit(Collider other)
    {
        _material.color = new Color(_color.r, _color.g, _color.b, _min);
    }

    void OnTriggerStay(Collider other)
    {
        //float distance = Vector3.Distance(transform.position, other.transform.position);
        float distance = Vector3.Dot(normalDirection, other.transform.position - transform.position);
        distance += parentCollider.center.y;

        if (distance < 0f)
            distance = 0f;
        if (distance > 30f)
            distance = 30f;

        distance = _max - distance;

        _material.color = new Color(_color.r, _color.g, _color.b, 0.8f * distance/_max);
    }
}
