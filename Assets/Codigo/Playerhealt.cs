
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida del jugador")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        // Aquí puedes poner animación de daño o muerte
        if (currentHealth <= 0)
        {
            Debug.Log("Jugador muerto");
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }
}
