using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform arrowTransform;
    private float arrowForce = 30f;

    public void FireArrow(float firePower)
    {
        arrowTransform.SetParent(null);
        Rigidbody rigidbody = arrowTransform.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.AddForce(transform.forward * arrowForce * firePower, ForceMode.Impulse);
    }
}
