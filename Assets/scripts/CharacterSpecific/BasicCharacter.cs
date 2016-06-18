using UnityEngine;
using System.Collections;

public class BasicCharacter : MonoBehaviour
{
    public float Health;
    public ParticleSystem explosionPrefab;

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    // Other is configured so it can either be another character or a bullet
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        var explosion = Instantiate(explosionPrefab);
        Vector3 pos = other.transform.position;
        pos.z = 100;
        explosion.transform.position = pos;

        Destroy(explosion.gameObject, explosion.startLifetime + explosion.startLifetime);

        if (other.gameObject.tag != "Player")
        {
            var character = other.GetComponent<BasicCharacter>();
            if (character != null)
                character.Kill();
            else // Not a player, probably a bullet
                Destroy(other.gameObject);
        }

        Health -= 1;
        if (Health <= 0)
            Kill();
    }
}
