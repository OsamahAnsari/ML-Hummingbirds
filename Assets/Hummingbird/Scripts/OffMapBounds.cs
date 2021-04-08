﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffMapBounds : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneHandler.RestartCurrentScene();
        }
    }
}
