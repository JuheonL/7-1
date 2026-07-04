using UnityEngine;

public class PlayerCollisionSound : MonoBehaviour
// Senses CharacterController, not RigidBody.
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    // Called when CharacterController collides with other collider
    // No Rigidbody required
    {
        NoisyObject noisy = hit.gameObject.GetComponent<NoisyObject>();
        if (noisy == null) return;
        noisy.TryEmitSound();
    }
}