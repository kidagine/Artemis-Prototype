using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : MonoBehaviour, IEnemy
{
    private const float damagedForce = 300f;

    public void Damage(Vector3 hitDirection)
    {
        Die(hitDirection);
    }

    private void Die(Vector3 hitDirection)
    {
        GetComponent<Rigidbody>().AddForce(hitDirection * damagedForce * 2);
    }
}
