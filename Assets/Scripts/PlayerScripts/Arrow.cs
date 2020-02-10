using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody arrowRigidbody;
    [SerializeField] private ParticleSystem hitEffectParticleSystem;
    [SerializeField] private ParticleSystem catchParticleSystem;
    [SerializeField] private Player player;
    [SerializeField] private Transform summonPoint;
    [SerializeField] private Material[] materials;
    private float summonSpeed = 2;
    private bool isSummoned;
    private const int rotateToTargetSpeed = 10;


    void Start()
    {
        arrowRigidbody.centerOfMass = new Vector3(0.054f, -0.006f, -0.155f);
    }

    void Update()
    {
        Summon();

    }
    
    private void Summon()
    {
        if (isSummoned)
        {
            float distance = Vector3.Distance(summonPoint.position, transform.position);
            if (distance < 1)
            {
                PickUp();
            }
            else
            {
                arrowRigidbody.Sleep();
                arrowRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                arrowRigidbody.isKinematic = true;
                summonSpeed += 15f * Time.deltaTime;

                float step = summonSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, summonPoint.position, step);
                Vector3 summonPointDirection = 2 * transform.position - summonPoint.position;
                Quaternion summonPointRotation = Quaternion.LookRotation(summonPointDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, summonPointRotation, rotateToTargetSpeed * Time.deltaTime);

                foreach (Material material in materials)
                {
                    material.SetFloat("_GlowPower", 0.03f);
                }
            }
        }
        else
        {
            foreach (Material material in materials)
            {
                material.SetFloat("_GlowPower", 0f);
            }
        }
    }

    private void PickUp()
    {
        isSummoned = false;
        player.GrabArrow();
        summonSpeed = 2;
        catchParticleSystem.Play();

        foreach (Material material in materials)
        {
            material.SetFloat("_GlowPower", 0f);
        }
    }

    public void SetSummon(bool state)
    {
        isSummoned = state;
        arrowRigidbody.isKinematic = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isSummoned)
        {
            AudioManager.Instance.Play("BounceArrow");
            hitEffectParticleSystem.Play();
        }
    }
}
