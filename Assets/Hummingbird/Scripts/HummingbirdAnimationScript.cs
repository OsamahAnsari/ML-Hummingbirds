using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HummingbirdAnimationScript : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float movementSpeed;

    public bool completedOneCycle = true;
    private bool reachedOneEnd = false;
    private Vector3 currentTargetPosition;

    public void Run()
    {
        completedOneCycle = false;
    }

    void Start()
    {
        currentTargetPosition = endPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.localPosition);
        if (transform.localPosition == currentTargetPosition)
        {
            reachedOneEnd = true;
            Debug.Log("Reached one end");
        }
        else
        {
            reachedOneEnd = false;
        }

        if (reachedOneEnd)
        {
            Debug.Log("Swapping");
            // If were moving to the end, swap and move back to start
            if (currentTargetPosition == endPosition.position)
            {
                currentTargetPosition = startPosition.position;
            }
            // Else we were moving to start, then we reached starting position again
            else
            {
                currentTargetPosition = endPosition.position;
                completedOneCycle = true;
            }            
        }

        if (!completedOneCycle)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentTargetPosition, movementSpeed * Time.deltaTime);
        }
    }
}
