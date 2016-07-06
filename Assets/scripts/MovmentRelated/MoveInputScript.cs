using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveInputScript : MonoBehaviour
{
    public PlayerScript player;
    public UInput input;
    public Canvas canvas;
    public GameObject prefab;

    public float speed = 400;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player == null)
            return;
        MoveHorizontally(input.GetHorizontalMovment());
    }

    private void MoveHorizontally(float factor)
    {
        player.SetSpeed(speed * factor);
    }
}