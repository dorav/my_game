using UnityEngine;
using System.Collections;

public class MovingTempObject : MonoBehaviour {

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
