using UnityEngine;
using UnityEngine.UI;

public class MenuPauseButtons : MonoBehaviour
{
    void OptionsButton()
    {
        Debug.Log("Options");
    }

    void MainMenuButton()
    {
        Debug.Log("Main menu à charger");
    }

    void ExitGameButton()
    {
        Application.Quit();
    }
}
