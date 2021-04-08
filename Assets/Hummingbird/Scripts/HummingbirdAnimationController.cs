using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HummingbirdAnimationController : MonoBehaviour
{
    public HummingbirdAnimationScript hummingbird1;
    public HummingbirdAnimationScript hummingbird2;
    public HummingbirdAnimationScript hummingbird3;

    public float timeBetween;
    public bool keepRunning = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(HummingbirdAnimations());
        }
    }

    IEnumerator HummingbirdAnimations()
    {
        yield return new WaitForSeconds(timeBetween / 2);
        hummingbird1.Run();
        yield return new WaitForSeconds(timeBetween);
        hummingbird2.Run();
        yield return new WaitForSeconds(timeBetween);
        hummingbird3.Run();
        yield return new WaitForSeconds(timeBetween / 2);
    }
}
