using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
    [SerializeField]
    private float thrust = 0.01f;
    public void ApplyForceToRigidBody(Rigidbody rb)
    {
        if (rb != null)
        {
            rb.AddForce(0, -thrust, 0, ForceMode.Impulse);
        }
    }
}
