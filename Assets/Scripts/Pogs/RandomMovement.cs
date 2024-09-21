using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField]
    private float maxRotationAngle = 8.0f;
    [SerializeField]
    private float moveAmount = 0.02f;
    public void ApplyRandomRotation()
    {
        float randomX = Random.Range(-maxRotationAngle, maxRotationAngle);
        float randomY = Random.Range(-maxRotationAngle, maxRotationAngle);
        float randomZ = Random.Range(-maxRotationAngle, maxRotationAngle);

        transform.Rotate(new Vector3(randomX, randomY, randomZ));
    }

    public void ApplyRandomMove()
    {
        float offsetX = Random.Range(-moveAmount, moveAmount);
        float offsetZ = Random.Range(-moveAmount, moveAmount);

        transform.position = transform.position + new Vector3(offsetX, 0, offsetZ);
    }
}
