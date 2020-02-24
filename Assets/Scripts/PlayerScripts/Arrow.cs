using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody arrowRigidbody;
    [SerializeField] private ParticleSystem hitEffectParticleSystem;
    [SerializeField] private ParticleSystem catchParticleSystem;
    [SerializeField] private Player player;
    [SerializeField] private Animator reticleAnimator;
    [SerializeField] private Transform summonPoint;
    [SerializeField] private Material[] materials;
    private GameObject stuckToEnemy;
    private float summonSpeed = 6;
    private const int summonMultiplier = 45;
    private bool isSummoned;
    private bool hasHit;
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
            if (stuckToEnemy != null)
            {
                stuckToEnemy.GetComponent<IEnemy>().Die();
                stuckToEnemy = null;
            }
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
                summonSpeed += summonMultiplier * Time.deltaTime;

                float step = summonSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, summonPoint.position, step);
                Vector3 summonPointDirection = 2 * transform.position - summonPoint.position;
                Quaternion summonPointRotation = Quaternion.LookRotation(summonPointDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, summonPointRotation, rotateToTargetSpeed * Time.deltaTime);

                foreach (Material material in materials)
                {
                    material.SetFloat("_GlowPower", 0.08f);
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
        hasHit = false;
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
        if (!arrowRigidbody.isKinematic)
        {
            IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();

            if (!isSummoned && !hasHit)
            {
                AudioManager.Instance.Play("BounceArrow");
                hitEffectParticleSystem.Play();
            }
            if (interactable != null)
            {
                interactable.Interact();
            }
            if (enemy != null && !hasHit)
            {
                AudioManager.Instance.Play("Kill");
                reticleAnimator.SetTrigger("Hit");
                hasHit = true;
                Vector3 hitDirection = other.contacts[0].point - transform.position;
                hitDirection = -hitDirection.normalized;
                int enemyhealth = enemy.Damage(hitDirection);
                if (enemyhealth == 0)
                {
                    foreach (Material material in materials)
                    {
                        material.SetFloat("_GlowPower", 0.08f);
                    }
                    stuckToEnemy = other.collider.gameObject;
                    arrowRigidbody.Sleep();
                    arrowRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    arrowRigidbody.isKinematic = true;
                }
            }
        }
    }
}
