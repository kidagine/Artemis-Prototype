using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform arrowTransform;
    [SerializeField] private Transform arrowPoint;
    [SerializeField] private ParticleSystem sparkParticleSystem;
    [SerializeField] private TrailRenderer trailEffect;
    private const float arrowForce = 25f;


    public void FireArrow(float firePower)
    {
        arrowTransform.SetParent(null);
        Rigidbody rigidbody = arrowTransform.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.AddForce(transform.forward * arrowForce * firePower, ForceMode.Impulse);

        sparkParticleSystem.Play();
        trailEffect.enabled = true;
    }

    public void EquipArrow()
    {
        arrowTransform.SetParent(arrowPoint);
        Rigidbody rigidbody = arrowTransform.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        trailEffect.enabled = false;
        arrowTransform.localPosition = Vector3.zero;
        arrowTransform.localRotation = Quaternion.identity;
    }
}
