using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public void Website () {
        System.Diagnostics.Process.Start("https://tomrijnbeek.nl");
    }

    public void LeaveRating () {
        System.Diagnostics.Process.Start("http://ludumdare.com/compo/author/cireon/");
    }

    public void Replay () {
        SceneManager.LoadScene("Main");
    }

    public void Exit () {
        Application.Quit();
    }
}
