using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{

    public Canvas canvasMainMenu;
    public Canvas canvasControllerSetup;
    public Canvas canvasCredits;

    public InputActionAsset firePlayer;
    public InputActionAsset waterPlayer;

    private GameObject pressAnyKey;

    // Start is called before the first frame update
    void Start()
    {
        canvasMainMenu.gameObject.SetActive(true);
        canvasControllerSetup.gameObject.SetActive(false);
        canvasCredits.gameObject.SetActive(false);
        pressAnyKey = canvasControllerSetup.transform.Find("PressAnyKey").gameObject;
        pressAnyKey.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {
        SceneManager.LoadScene("RaceScene");
    }

    public void ControllerSetup()
    {
        canvasMainMenu.gameObject.SetActive(false);
        canvasControllerSetup.gameObject.SetActive(true);
    }

    public void Credits()
    {
        canvasMainMenu.gameObject.SetActive(false);
        canvasCredits.gameObject.SetActive(true);
    }

    public void Back()
    {
        canvasControllerSetup.gameObject.SetActive(false);
        canvasCredits.gameObject.SetActive(false);
        canvasMainMenu.gameObject.SetActive(true);
    }

    public void BindFireKey()
    {
        GameObject button = null;
        InputAction action = null;
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (clickedButton.CompareTag("Fire"))
        {
            button = canvasControllerSetup.transform.Find("ButtonSetFirePFire").gameObject;
            action = firePlayer.FindAction("Fire");
        }
        if (clickedButton.CompareTag("Water"))
        {
            button = canvasControllerSetup.transform.Find("ButtonSetWaterPFire").gameObject;
            action = waterPlayer.FindAction("Fire");
        }
        if (button == null || action == null)
            return;
        button.SetActive(false);
        pressAnyKey.SetActive(true);
        action.Disable();

        var rebindOperation = action.PerformInteractiveRebinding().Start();
        string keyString;

        rebindOperation.OnComplete(
        operation =>
        {
            keyString = $"{operation.selectedControl}";
            operation.Dispose();
            button.SetActive(true);
            pressAnyKey.SetActive(false);

            action.AddBinding(keyString);
            action.Enable();
        });

        rebindOperation.Start();
    }

    public void BindJumpKey()
    {
        GameObject button = null;
        InputAction action = null;
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (clickedButton.CompareTag("Fire"))
        {
            button = canvasControllerSetup.transform.Find("ButtonSetFirePJump").gameObject;
            action = firePlayer.FindAction("Jump");
        }
        if (clickedButton.CompareTag("Water"))
        {
            button = canvasControllerSetup.transform.Find("ButtonSetWaterPJump").gameObject;
            action = waterPlayer.FindAction("Jump");
        }
        if (button == null || action == null)
            return;
        button.SetActive(false);
        pressAnyKey.SetActive(true);
        action.Disable();

        var rebindOperation = action.PerformInteractiveRebinding().Start();
        string keyString;

        rebindOperation.OnComplete(
        operation =>
        {
            keyString = $"{operation.selectedControl}";
            operation.Dispose();
            button.SetActive(true);
            pressAnyKey.SetActive(false);

            action.AddBinding(keyString);
            action.Enable();
        });

        rebindOperation.Start();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
