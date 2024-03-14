using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallAudio : MonoBehaviour
{
    void Start()
    {
        Invoke("Destroy", 1.5f);
    }
}
