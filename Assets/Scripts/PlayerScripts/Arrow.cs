using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private ParticleSystem hitEffectParticleSystem;
    [SerializeField] private TrailRenderer trailEffect;
    private Transform target;
    private bool isSummoned;
    private bool hasLanded;

    void Update()
    {
        if (isSummoned)
        {
            float step = 8 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            transform.rotation = Quaternion.LookRotation(target.position);
        }
    }

    public void Summon(bool state, Transform target)
    {
        isSummoned = state;
        this.target = target;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hasLanded)
        {
            hitEffectParticleSystem.Play();
            if (other.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                hasLanded = true;
                trailEffect.enabled = false;
                AudioManager.Instance.Play("BounceArrow");
                //GetComponent<Rigidbody>().Sleep();
                //GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                //GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        if (hasLanded)
        {
            if (other.collider.gameObject.CompareTag("Player"))
            {
                isSummoned = false;
                hasLanded = false;
                Player player = other.collider.GetComponent<Player>();
                player.GrabArrow();
            }
        }
    }
}
