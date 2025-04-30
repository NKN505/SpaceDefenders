using UnityEngine;
using System;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Salud")]
    public int maxHealth = 40;
    private int currentHealth;
    public event Action<EnemyHealth> onDeath;
    public int enemyScore = 0;
    private bool isDead = false;

    [Header("Efecto visual")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.15f;
    private Color originalColor;
    private Color originalEmission;
    private SkinnedMeshRenderer rend;
    private MaterialPropertyBlock propBlock;

    [Header("Explosión al morir")]
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public float explosionSoundVolume = 1f;
    public float explosionSoundRange = 20f; // 🔊 nuevo campo

    private void Awake()
    {
        currentHealth = maxHealth;

        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        if (rend == null)
        {
            Debug.LogError("SkinnedMeshRenderer no encontrado en hijos.");
            enabled = false;
            return;
        }

        Material mat = rend.sharedMaterial;
        originalColor = mat.HasProperty("_BaseColor") ? mat.GetColor("_BaseColor") : Color.white;
        originalEmission = mat.HasProperty("_EmissionColor") ? mat.GetColor("_EmissionColor") : Color.black;

        propBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        isDead = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} de daño. Salud restante: {currentHealth}");

        StartCoroutine(Flash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator Flash()
    {
        rend.GetPropertyBlock(propBlock);

        propBlock.SetColor("_BaseColor", flashColor);
        propBlock.SetColor("_EmissionColor", flashColor * 2f);

        rend.SetPropertyBlock(propBlock);

        yield return new WaitForSeconds(flashDuration);

        propBlock.SetColor("_BaseColor", originalColor);
        propBlock.SetColor("_EmissionColor", originalEmission);

        rend.SetPropertyBlock(propBlock);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} ha muerto. Disparando evento onDeath.");
        onDeath?.Invoke(this);

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        if (explosionSound != null)
        {
            GameObject soundGO = new GameObject("ExplosionSound");
            soundGO.transform.position = transform.position;

            AudioSource audioSource = soundGO.AddComponent<AudioSource>();
            audioSource.clip = explosionSound;
            audioSource.volume = explosionSoundVolume;
            audioSource.spatialBlend = 1f; // Sonido 3D
            audioSource.maxDistance = explosionSoundRange;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.Play();

            Destroy(soundGO, explosionSound.length);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        onDeath = null;
    }

    // 🔵 Gizmo para visualizar el rango del sonido en escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, explosionSoundRange);
    }
}
