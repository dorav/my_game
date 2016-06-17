using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveInputScript : MonoBehaviour
{
    public PlayerScript player;
    public IInput input;
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

        if (input.IsMovingRight())
            moveRight();
        else if (input.IsMovingLeft())
            moveLeft();
        else
        {
            stopMovment();
        }

    }

    private void moveRight()
    {
        player.SetSpeed(speed);
    }

    private void moveLeft()
    {
        player.SetSpeed(-speed);
    }

    private void stopMovment()
    {
        player.SetSpeed(0);
    }
}