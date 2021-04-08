using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        gameObject.transform.eulerAngles = player.transform.eulerAngles;
    }

    private void LateUpdate()
    {
        gameObject.transform.eulerAngles = player.transform.eulerAngles;
    }
}
