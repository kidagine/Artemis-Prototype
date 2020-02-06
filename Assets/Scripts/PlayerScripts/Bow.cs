using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform arrowTransform;
    [SerializeField] private ParticleSystem sparkParticleSystem;
    private const float arrowForce = 30f;

    public void FireArrow(float firePower)
    {
        sparkParticleSystem.Play();
        arrowTransform.SetParent(null);
        Rigidbody rigidbody = arrowTransform.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.AddForce(transform.forward * arrowForce * firePower, ForceMode.Impulse);
    }
}
