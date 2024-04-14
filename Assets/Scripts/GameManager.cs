using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject Ally_Circle;
    public GameObject Enemy_Circle;
    
    public EndGameController endGameController;
    public TextMeshProUGUI scritta;

    void Start()
    {
        OSTManager.Instance.PlayMain();
        Health allyHealth = Ally_Circle.GetComponent<Health>();
        Health enemyHealth = Enemy_Circle.GetComponent<Health>();

        if (allyHealth != null)
            allyHealth.OnDeath += HandleAllyDeath;

        if (enemyHealth != null)
            enemyHealth.OnDeath += HandleEnemyDeath;
    }

    private void HandleAllyDeath()
    {
        GameOver(false);
        if (endGameController)
        {
            endGameController.EndGame(false);
        }
    }

    private void HandleEnemyDeath()
    {
        GameOver(true);
        endGameController.EndGame(true);
    }

    private void GameOver(bool win)
    {
        if (win)
        {
            OSTManager.Instance.PlayCalm();
        }
        else
        {
            OSTManager.Instance.PlayCalm();
        }

    }

    void OnDestroy()
    {
        if (Ally_Circle)
        {
            Health allyHealth = Ally_Circle.GetComponent<Health>();
            if (allyHealth != null)
                allyHealth.OnDeath -= HandleAllyDeath;
        }

        if (Enemy_Circle)
        {
            Health enemyHealth = Enemy_Circle.GetComponent<Health>();
            if (enemyHealth != null)
                enemyHealth.OnDeath -= HandleEnemyDeath;
        }
    }
}