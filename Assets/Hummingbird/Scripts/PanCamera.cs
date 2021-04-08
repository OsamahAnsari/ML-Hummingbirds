using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
    public GameObject startPosition;
    public GameObject endPosition;
    public float moveByZUnits;
    public bool panBack = true;

    private Vector3 start;
    private Vector3 end;
    private bool reachedOtherEnd = false;
    
    // Start is called before the first frame update
    void Start()
    {
        start = startPosition.transform.position;
        end = endPosition.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (moveByZUnits > 0 && gameObject.transform.position.z >= end.z 
            || moveByZUnits < 0 && gameObject.transform.position.z <= start.z)
        {
            reachedOtherEnd = true;
        }
        else
        {
            reachedOtherEnd = false;
        }

        if (reachedOtherEnd && panBack)
        {
            moveByZUnits *= -1;
        }
        else if (reachedOtherEnd && !panBack)
        {
            moveByZUnits = 0;
        }

        gameObject.transform.position += new Vector3(0, 0, moveByZUnits * Time.deltaTime);
    }
}
