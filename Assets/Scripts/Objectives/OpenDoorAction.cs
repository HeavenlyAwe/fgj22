using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorAction : CallbackAction
{
    public GameObject door;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator OpenDoor()
    {
        for (float time = 0; time < 3.0f; time += Time.deltaTime)
        {
            door.transform.Rotate(Vector3.up, time);
            yield return null;
        }
        door.SetActive(false);
    }

    public override void Callback()
    {
        if (door == null) return;

        StartCoroutine(OpenDoor());
    }
}
