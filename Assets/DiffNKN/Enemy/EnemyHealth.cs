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

    private void Awake()
    {
        currentHealth = maxHealth;

        // Busca el SkinnedMeshRenderer en hijos
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        if (rend == null)
        {
            Debug.LogError("SkinnedMeshRenderer no encontrado en hijos.");
            enabled = false;
            return;
        }

        // Obtiene los colores originales desde el material
        Material mat = rend.sharedMaterial;
        if (mat.HasProperty("_BaseColor")) originalColor = mat.GetColor("_BaseColor");
        else originalColor = Color.white;

        if (mat.HasProperty("_EmissionColor")) originalEmission = mat.GetColor("_EmissionColor");
        else originalEmission = Color.black;

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

        // Activa el flash
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
        propBlock.SetColor("_EmissionColor", flashColor * 2f); // Emission más brillante

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
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        onDeath = null;
    }
}
