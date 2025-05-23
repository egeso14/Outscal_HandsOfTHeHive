﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyBee : MonoBehaviour
{
    Animator bee;
    IEnumerator coroutine;

	void Start ()
    {
        bee = GetComponent<Animator>();
	}
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            bee.SetBool("walk", true);
            bee.SetBool("idle", false);
            bee.SetBool("fly", true);
            bee.SetBool("flyinplace", false);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            bee.SetBool("idle", true);
            bee.SetBool("walk", false);
            bee.SetBool("fly", false);
            bee.SetBool("flyinplace", true);
        }
        if (bee.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            bee.SetBool("flyinplace", false);
            bee.SetBool("fly", false);
            bee.SetBool("landing", false);
            bee.GetComponent<AudioSource>().enabled = false;
        }
        if (bee.GetCurrentAnimatorStateInfo(0).IsName("landing"))
        {
            bee.SetBool("takeoff", false);
        }

        if (bee.GetCurrentAnimatorStateInfo(0).IsName("fly"))
        {
            bee.SetBool("flyinplace", false);
            bee.SetBool("idle", false);
            bee.SetBool("landing", false);
        }
        if (bee.GetCurrentAnimatorStateInfo(0).IsName("flyinplace"))
        {
            bee.SetBool("fly", false);
            bee.SetBool("idle", false);
            bee.SetBool("landing", false);
            bee.SetBool("takeoff", false);
            bee.SetBool("hit", false);
            bee.SetBool("attack", false);
            bee.GetComponent<AudioSource>().enabled = true;
        }
        if (bee.GetCurrentAnimatorStateInfo(0).IsName("die"))
        {
            bee.GetComponent<AudioSource>().enabled = false;
        }
        if (bee.GetCurrentAnimatorStateInfo(0).IsName("takeoff"))
        {
            bee.GetComponent<AudioSource>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bee.SetBool("landing", true);
            bee.SetBool("takeoff", true);
            bee.SetBool("fly", false);
            bee.SetBool("flyinplace", false);
            bee.SetBool("flyleft", false);
            bee.SetBool("flyright", false);
            if ((bee.GetCurrentAnimatorStateInfo(0).IsName("fly"))||(bee.GetCurrentAnimatorStateInfo(0).IsName("landing"))||(bee.GetCurrentAnimatorStateInfo(0).IsName("flyinplace")))
            {
                bee.GetComponent<AudioSource>().enabled = !bee.GetComponent<AudioSource>().enabled;
                Debug.Log("fly is not current");
            }
            //StartCoroutine("idle", true);
            //idle();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bee.SetBool("idle", false);
            bee.SetBool("takeoff", true);
            if (bee.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                bee.GetComponent<AudioSource>().enabled = !bee.GetComponent<AudioSource>().enabled;
                Debug.Log("idle is not current");
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            bee.SetBool("fly", true);
            bee.SetBool("flyinplace", false);
        }
        if ((Input.GetKeyUp(KeyCode.A))||(Input.GetKeyUp(KeyCode.D)))
        {
            bee.SetBool("flyleft", false);
            bee.SetBool("flyright", false);
            bee.SetBool("turnleft", false);
            bee.SetBool("turnright", false);
            bee.SetBool("fly", true);
            bee.SetBool("idle", true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            bee.SetBool("fly", false);
            bee.SetBool("idle", false);
            bee.SetBool("flyright", false);
            bee.SetBool("flyleft", true);
            bee.SetBool("turnleft", true);
            bee.SetBool("flyinplace", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            bee.SetBool("fly", false);
            bee.SetBool("idle", false);
            bee.SetBool("flyleft", false);
            bee.SetBool("flyright", true);
            bee.SetBool("turnright", true);
            bee.SetBool("flyinplace", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            bee.SetBool("fly", false);
            bee.SetBool("flyleft", false);
            bee.SetBool("flyright", false);
            bee.SetBool("flyinplace", true);
        }
        if (Input.GetMouseButton(1))
        {
            bee.SetBool("hit", true);
            bee.SetBool("fly", false);
            bee.SetBool("flyleft", false);
            bee.SetBool("flyright", false);
            bee.SetBool("flyinplace", false);
            bee.SetBool("idle", false);
        }
        if (Input.GetMouseButton(2))
        {
            bee.SetBool("fly", false);
            bee.SetBool("flyleft", false);
            bee.SetBool("flyright", false);
            bee.SetBool("flyinplace", false);
            bee.SetBool("idle", false);
            bee.SetBool("die", true);
        }
        if (Input.GetMouseButton(0))
        {
            bee.SetBool("attack", true);
            bee.SetBool("fly", false);
            bee.SetBool("flyleft", false);
            bee.SetBool("flyright", false);
            bee.SetBool("flyinplace", false);
            bee.SetBool("idle", false);

        }
	}
    IEnumerator flyinplace()
    {
        yield return new WaitForSeconds(0.3f);
        bee.SetBool("flyinplace", true);
        bee.SetBool("takeoff", false);
        bee.SetBool("hit", false);
        bee.SetBool("attack", false);
        bee.SetBool("idle", false);
    }
    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.5f);
        bee.SetBool("landing", false);
        bee.SetBool("idle", true);
    }

}
