using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private ParticleSystem hitEffectParticleSystem;
    [SerializeField] private ParticleSystem hitEffectParticleSystem1;
    [SerializeField] private TrailRenderer trailEffect;
    [SerializeField] private Transform summonPoint;
    [SerializeField] private Material[] materials;
    private Transform target;
    private float summonSpeed = 2;
    private bool isSummoned;
    private bool hasLanded;
    private const int rotateToTargetSpeed = 10;


    void Start()
    {
        rigidbody.centerOfMass = new Vector3(0.054f, -0.006f, -0.155f);
    }

    void Update()
    {
        if (isSummoned)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance < 1)
            {
                Player player = target.GetComponent<Player>();
                Pickup(player);

                foreach (Material material in materials)
                {
                    material.SetFloat("_GlowPower", 0f);
                }
                summonSpeed = 2;
                hitEffectParticleSystem1.Play();
            }
            else
            {
                rigidbody.Sleep();
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rigidbody.isKinematic = true;
                summonSpeed += 15f * Time.deltaTime;
                float step = summonSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, summonPoint.position, step);
                Vector3 dir = 2 * transform.position - summonPoint.position;
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateToTargetSpeed * Time.deltaTime);

                foreach (Material material in materials)
                {
                    material.SetFloat("_GlowPower", 0.03f);
                }
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = transform.position + Random.insideUnitSphere * 0.05f;
        }
    }

    public void Summon(bool state, Transform target)
    {
        isSummoned = state;
        this.target = target;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Pickup(Player player)
    {
        trailEffect.enabled = false;
        isSummoned = false;
        hasLanded = false;
        player.GrabArrow();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hasLanded)
        {
            if (other.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                hasLanded = true;
                //GetComponent<Rigidbody>().Sleep();
                //GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                //GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        if (hasLanded)
        {
            if (other.collider.gameObject.CompareTag("Player"))
            {
                //Player player = other.collider.GetComponent<Player>();
                //Pickup(player);

                //foreach (Material material in materials)
                //{
                //    material.SetFloat("_GlowPower", 0f);
                //}
            }
            else
            {
                if (!isSummoned)
                {
                    hitEffectParticleSystem.Play();
                    AudioManager.Instance.Play("BounceArrow");
                }
            }
        }
    }
}
