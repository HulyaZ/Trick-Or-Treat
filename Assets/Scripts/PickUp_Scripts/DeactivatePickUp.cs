using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatePickUp : MonoBehaviour
{
    void Start()
    {
        Deactivate();
    }

   void Deactivate()
    {
        Destroy(gameObject, 3f);
    }
}
