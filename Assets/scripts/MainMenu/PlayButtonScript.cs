using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayButtonScript : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("SurvivalMode");
    }
}
