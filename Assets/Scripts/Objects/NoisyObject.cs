using UnityEngine;

public class NoisyObject : MonoBehaviour
{
    [Header("소리 설정")]
    public float soundIntensity = 0.8f;
    public float cooldown = 1.0f;

    [Header("연출 (선택)")]
    public AudioClip soundClip;
    private AudioSource _audio;

    private float _lastSoundTime = -999f;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    // Only senses Rigidbody based collisons, no Charactercontroller
    // When two collider collides and either has Rigidbody.
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        TryEmitSound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        TryEmitSound();
    }

    public void TryEmitSound()
    {
        if (Time.time - _lastSoundTime < cooldown) return;
        _lastSoundTime = Time.time;

        SoundEvent.Emit(transform.position, soundIntensity);

        if (_audio != null && soundClip != null)
            _audio.PlayOneShot(soundClip);
    }
}