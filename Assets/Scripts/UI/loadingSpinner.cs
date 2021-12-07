using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadingSpinner : MonoBehaviour
{
    [SerializeField] private float rotatateSpeed;
    private void Update()
    {
        transform.Rotate(-Vector3.forward * Time.deltaTime * rotatateSpeed);
    }

}
