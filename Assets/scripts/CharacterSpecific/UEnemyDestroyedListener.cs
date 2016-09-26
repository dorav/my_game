using UnityEngine;

public abstract class UEnemyDestroyedListener : MonoBehaviour
{
    private static int numberOfActiveEnemies;

    public static int NumberOfActiveEnemies
    {
        get { return numberOfActiveEnemies; }
        set
        {
            numberOfActiveEnemies = value;
            if (OnNumberOfActiveEnemiesChanged != null)
                OnNumberOfActiveEnemiesChanged();
        }
    }

    public delegate void NumberOfActiveEnemiesChanged_();
    public static event NumberOfActiveEnemiesChanged_ OnNumberOfActiveEnemiesChanged;

    public delegate void EnemyDestroyed_(EnemyShooter destroyed);
    public static event EnemyDestroyed_ OnBeforeEnemyDestroyed;

    public static void ReportEnemyDestroyed(EnemyShooter destroyed)
    {
        if (OnBeforeEnemyDestroyed != null)
            OnBeforeEnemyDestroyed(destroyed);
        NumberOfActiveEnemies--;
    }
}
