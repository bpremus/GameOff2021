using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slow_rotate : MonoBehaviour
{
    [SerializeField]
    GameObject rotation_elem;
    // Start is called before the first frame update
    [SerializeField]
    float rot_speed = 20;

    public void Update()
    {
        transform.RotateAround(rotation_elem.transform.position, new Vector3(0,0,1), rot_speed * Time.deltaTime);
    }
}
