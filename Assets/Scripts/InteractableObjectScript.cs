using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectScript : MonoBehaviour
{
    public Transform playerTransform;
    public LayerMask playerMask;
    public Light pointLight;

    public float radius;
    public float activeRange;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        pointLight.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, playerTransform.position) < activeRange)
        {
            Interaction();
        }
    }

    private bool Interaction()
    {
        // check for player close enough and key input
        bool playerDetect = Physics.CheckSphere(transform.position, radius, playerMask);
        if (playerDetect && Input.GetKeyDown(KeyCode.E))
        {
            // do interaction
            player.GetComponent<PlayerControl>().AddToKeyCount();
            // remove gameobject
            this.gameObject.SetActive(false);
            return true;
        }
        else if (!playerDetect)
        {
            pointLight.color = Color.white;
            return false;
        }
        else
        {
            return false;
        }

    }


}
