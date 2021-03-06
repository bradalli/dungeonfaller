﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrigger : MonoBehaviour
{
    [SerializeField] GameManager gMang;
    [SerializeField] PlayerContoller playCont;
    [SerializeField] Material impactMat;

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("IMPACT");

        switch (col.tag)
        {
            case "Obstacle":
                gMang.StartCoroutine(gMang.DEATH());
                col.transform.Find("Outline").gameObject.SetActive(true);
                break;

            case "Destructable":
                if (playCont.fast) { Destroy(col.gameObject); }
                else { gMang.StartCoroutine(gMang.DEATH()); col.transform.Find("Outline").gameObject.SetActive(true); }
                break;

            case "Coin":
                gMang.coinAmount++;
                Destroy(col.gameObject);
                break;

        }
    }
}
