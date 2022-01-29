using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public Canvas canvasMainMenu;
    public Canvas canvasControllerSetup;

    // Start is called before the first frame update
    void Start()
    {
        canvasMainMenu.gameObject.SetActive(true);
        canvasControllerSetup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {
        SceneManager.LoadScene("DevToffe");
    }

    public void ControllerSetup()
    {
        canvasMainMenu.gameObject.SetActive(false);
        canvasControllerSetup.gameObject.SetActive(true);
    }

    public void BindKey()
    {
        Debug.Log("BindKey");
    }

    public void Back()
    {
        canvasControllerSetup.gameObject.SetActive(false);
        canvasMainMenu.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
