
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool isInvicible;

    public int currentHealt;
    public int maxHealt = 1;
    public int contactDamage = 1;

    //Start is called before the first frame update 
    void start
    {
        currentHealt = maxHealt;
    }

    public void Invincible(bool invincibility)
    {
        isInvicible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        if (isInvicible)
        {
            currentHealt -= damage;
            MathF.Clamp(currentHealt, 0, maxHealt);
            if (currentHealt <= 0)
            {
                Defeat();
            }
        }
    }

    void Defeat()
    {
        Destroy(GameObject);
    }

    private void OntriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit ");
        }
    }

}
