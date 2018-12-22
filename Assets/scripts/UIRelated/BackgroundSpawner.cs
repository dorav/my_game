using UnityEngine;
using System.Collections;

public class BackgroundSpawner : MonoBehaviour {

    public float RespawnTime;

    public GameObject backgroundSpawnPrefab;
    public Canvas canvas;
    public Sprite[] backgroundSpawns;
    public float speedRatio = 15;

    float RespawnCooldown = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RespawnCooldown -= Time.deltaTime;
	    if (RespawnCooldown < 0)
        {
            GameObject backgroundSpawn = Instantiate(backgroundSpawnPrefab);

            int randomSprite = Random.Range(0, backgroundSpawns.Length);
            Sprite chosenSprite = backgroundSpawns[randomSprite];
            float scaleSpeedRatio = Random.Range(1f, 1.5f);
            scaleSpeedRatio *= scaleSpeedRatio;
            backgroundSpawn.GetComponent<SpriteRenderer>().sprite = chosenSprite;
            var scaleFactor = scaleSpeedRatio * 150;
            backgroundSpawn.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(scaleFactor, scaleFactor);
            backgroundSpawn.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -scaleSpeedRatio * speedRatio);

            Vector3 spawnPos = CameraUtils.RandomSpawnPointAboveView(backgroundSpawn.GetComponent<Renderer>());
            backgroundSpawn.transform.position = spawnPos;

            RespawnCooldown = RespawnTime;
        }
    }
}
