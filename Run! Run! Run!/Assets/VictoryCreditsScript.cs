using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class VictoryCreditsScript : MonoBehaviour
{
    public Animator anim;

    public void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RollCredits()
    {
        anim.SetBool("Credits", true);
    }
}
