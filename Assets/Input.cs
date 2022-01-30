using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{

    public Canvas canvasControllerSetup;

    private GameObject firePFireActive, waterPFireActive, firePJumpActive, waterPJumpActive;
    private float fireFireTimer = 0;
    private float waterFireTimer = 0;
    private float fireJumpTimer = 0;
    private float waterJumpTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        firePFireActive = canvasControllerSetup.transform.Find("FirePFireActive").gameObject;
        firePFireActive.SetActive(false);

        waterPFireActive = canvasControllerSetup.transform.Find("WaterPFireActive").gameObject;
        waterPFireActive.SetActive(false);

        firePJumpActive = canvasControllerSetup.transform.Find("FirePJumpActive").gameObject;
        firePJumpActive.SetActive(false);

        waterPJumpActive = canvasControllerSetup.transform.Find("WaterPJumpActive").gameObject;
        waterPJumpActive.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (fireFireTimer > 0)
            fireFireTimer -= Time.deltaTime;
        if (fireFireTimer < 0)
            firePFireActive.SetActive(false);

        if (waterFireTimer > 0)
            waterFireTimer -= Time.deltaTime;
        if (waterFireTimer < 0)
            waterPFireActive.SetActive(false);

        if (fireJumpTimer > 0)
            fireJumpTimer -= Time.deltaTime;
        if (fireJumpTimer < 0)
            firePJumpActive.SetActive(false);

        if (waterJumpTimer > 0)
            waterJumpTimer -= Time.deltaTime;
        if (waterJumpTimer < 0)
            waterPJumpActive.SetActive(false);
    }

    public void OnFire()
    {
        if (gameObject.CompareTag("Fire"))
        {
            firePFireActive.SetActive(true);
            fireFireTimer = .2f;
        }
        if (gameObject.CompareTag("Water"))
        {
            waterPFireActive.SetActive(true);
            waterFireTimer = .2f;
        }
    }

    public void OnJump()
    {
        if (gameObject.CompareTag("Fire"))
        {
            firePJumpActive.SetActive(true);
            fireJumpTimer = .2f;
        }
        if (gameObject.CompareTag("Water"))
        {
            waterPJumpActive.SetActive(true);
            waterJumpTimer = .2f;
        }
    }
}
