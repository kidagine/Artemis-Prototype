using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            AudioManager.Instance.Play("BounceArrow");
            //print(other.gameObject.name);
            //GetComponent<Rigidbody>().Sleep();
            //GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            //GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
