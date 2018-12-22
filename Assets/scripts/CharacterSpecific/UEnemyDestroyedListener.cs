using System.Threading;
using UnityEngine;

public abstract class UEnemyDestroyedListener : MonoBehaviour
{
    private static int numberOfActiveEnemies;

    public delegate void NumberOfActiveEnemiesChanged_();
    public static event NumberOfActiveEnemiesChanged_ OnNumberOfActiveEnemiesChanged;

    public delegate void EnemyDestroyed_(EnemyShooter destroyed);
    public static event EnemyDestroyed_ OnBeforeEnemyDestroyed;

    public static void ReportEnemyDestroyed(EnemyShooter destroyed)
    {
        if (OnBeforeEnemyDestroyed != null)
            OnBeforeEnemyDestroyed(destroyed);

        Interlocked.Decrement(ref numberOfActiveEnemies);

        if (OnNumberOfActiveEnemiesChanged != null)
            OnNumberOfActiveEnemiesChanged();
    }

    public static void setNumberOfEnemies(int newNumberOfEnemies)
    {
        numberOfActiveEnemies = newNumberOfEnemies;
    }

    public static void reportNewEnemy()
    {
        Interlocked.Increment(ref numberOfActiveEnemies);
        if (OnNumberOfActiveEnemiesChanged != null)
            OnNumberOfActiveEnemiesChanged();
    }

    public static int NumberOfActiveEnemies
    {
        get { return numberOfActiveEnemies; }
    }
}
