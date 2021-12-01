using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandBugs : MonoBehaviour
{

    // Start is called before the first frame update

    Vector3 _position;

    void Start()
    {
        _position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.ISDayCycle())
        {
            transform.position = Vector3.Slerp(transform.position, _position, Time.deltaTime * 3f);
        }
        else
        {
            Vector3 hid_pos = _position - Vector3.up * 2;
            transform.position = Vector3.Slerp(transform.position, hid_pos, Time.deltaTime * 3f);
        }
    }
}
