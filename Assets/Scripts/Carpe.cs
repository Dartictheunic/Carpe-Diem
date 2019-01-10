﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using FantomLib;

public class Carpe : MonoBehaviour {

    [Header("Variables à tweak pour le gamefeel")]
    [Space(10)]

    [Header("Movement")]
    [Tooltip("Vitesse à laquelle la carpe se déporte sur le côté")]
    [Range(0f, 1f)]
    public float moveSpeed;

    [Header("Jump")]
    [Tooltip("Vitesse à laquelle la carpe saute")]
    [Range(0f, 1f)]
    public float jumpSpeed;

    [Tooltip("Vitesse à laquelle la carpe redescend du saut")]
    [Range(0f, .4f)]
    public float fallSpeed;

    [Tooltip("Hauteur de saut de la carpe")]
    [Range(0f, 3f)]
    public float jumpHeight;

    [Tooltip("Distance à laquelle le style augmente")]
    [Range(0f, 2f)]
    public float styleDetectionDistance;

    [Space(10)]

    public Text carpeText;

    [Header("Stomp")]

    [Tooltip("Vitesse à laquelle la carpe stomp")]
    [Range(0f, 1f)]
    public float stompSpeed;

    [Space(10)]

    [Header("Impacts")]
    [Tooltip("Recul dû aux impacts")]
    public float hurtForce;

    [Tooltip("Vitesse de l'impact")]
    [Range(0f, 1f)]
    public float hurtSpeed;

    [Tooltip("Vitesse pour revenir au point de départ")]
    [Range(0f, 1f)]
    public float recoverySpeed;

    [Tooltip("Temps d'invincibilité")]
    public float invicibilityTime;

    [Tooltip("Alpha minimum lorsqu'on se fait hit")]
    [Range(0f, 1f)]
    public float minimumAlpha;


    [Space(20)]
    [Header("Trucs à link manuellement")]
    public CarpeManager carpeManager;
    public Animator carpeAnimator;
    public SkinnedMeshRenderer carpeMesh;

    [Space(10)]
    [Header("Variables pour la prog")]
    public bool canJump;
    public bool isJumping;
    public bool canBeHurt;
    public bool isHurt;
    public bool isRecovering;
    public CarpeState carpeState;

    Sequence invisibilityBlink;
    bool isMaterialBlinking;
    float invincibilityLeft;
    Rigidbody body;
    Material mat;
    Vector3 hurtPos;
    List<Material> carpeMats;

    Vector3 basePos;

    public void MoveOut(float xDestination)
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(xDestination, transform.localPosition.y, transform.localPosition.z), moveSpeed);
    }

    public void MoveIn()
    {
        Debug.Log(name);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(basePos.x, transform.localPosition.y, transform.localPosition.z), moveSpeed);
    }

    private void Start()
    {
        carpeMats = new List<Material>();
        invisibilityBlink = DOTween.Sequence();
        body = GetComponent<Rigidbody>();
        mat = GetComponent<MeshRenderer>().material;
        basePos = transform.localPosition;
        int numOfChildren = transform.childCount;
        for (int i = 0; i < carpeMesh.materials.Length; i++)
        {
            carpeMats.Add(carpeMesh.materials[i]);
        }
    }

    public void Jump()
    {
        if (carpeState == CarpeState.floating || carpeState == CarpeState.recovering)
        {
            RaycastHit hit;

            if (Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z), transform.TransformDirection(Vector3.forward), out hit, 20f))
            {
                Debug.DrawLine(new Vector3(transform.position.x, 0, transform.position.z), transform.TransformDirection(Vector3.forward) * 150, Color.red, 5f );
                    Debug.Log(hit.distance);
                if (hit.collider.GetComponent<Obstacles>() != null && hit.distance < styleDetectionDistance)
                {
                    carpeManager.ObstaclePassed(hit.collider.GetComponent<Obstacles>().ObstacleValue);
                    carpeManager.IncreaseScore(Mathf.FloorToInt(1 / hit.distance));
                }

            }

            carpeAnimator.SetTrigger("Jump");

            carpeState = CarpeState.startJump;
        }

        //else if (carpeState == CarpeState.endJump || carpeState == CarpeState.startJump)
        //{
        //    RaycastHit hit;

        //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        //    {
        //        if (hit.distance < styleDetectionDistance)
        //            carpeManager.UpdateMultiplier(true);
        //    }

        //    carpeState = CarpeState.stomp;
        //}
    }

    public void Hurt()
    {
        if (canBeHurt)
        {
            #if !UNITY_EDITOR
                AndroidPlugin.StartVibrator(60);
            #endif
    
            carpeManager.UpdateMultiplier(false);
            
            foreach(Material meeesh in carpeMats)
            {
                invisibilityBlink.Append(meeesh.DOFade(minimumAlpha, "_Color", invicibilityTime/6).SetLoops(6, LoopType.Yoyo));
                invisibilityBlink.Append(meeesh.DOColor(meeesh.color, .1f));
            }
            carpeState = CarpeState.hurt;
            canBeHurt = false;
            invincibilityLeft = invicibilityTime;
            body.velocity = Vector3.zero;
            hurtPos.z = transform.localPosition.z - hurtForce;
        }
        
    }
	
	void FixedUpdate () {
            switch(carpeState)
            {
                case CarpeState.floating:
                    {
                        transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, new Vector3(transform.localPosition.x, 0, 0), recoverySpeed);
                    }
                    break;

                case CarpeState.startJump:
                    {
                        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, jumpHeight, transform.localPosition.z), jumpSpeed);
                        if (FastApproximately(transform.localPosition.y, jumpHeight))
                        {
                            carpeState = CarpeState.endJump;
                        }
                    }
                    break;

                case CarpeState.endJump:
                    {
                        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), fallSpeed);
                        if (FastApproximatelyPrecise(transform.localPosition.y, 0))
                        {
                            canJump = true;
                            carpeState = CarpeState.floating;
                        }
                    }
                    break;

                case CarpeState.stomp:
                    {
                        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), stompSpeed);
                        if (FastApproximately(transform.localPosition.y, 0))
                        {
                            carpeState = CarpeState.floating;
                        }
                    }
                    break;

                case CarpeState.hurt:
                    {
                        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, hurtPos.z), hurtSpeed);
                        if (FastApproximatelyPrecise(transform.localPosition.z, -hurtForce))
                        {
                            carpeState = CarpeState.recovering;
                        }
                        
                    }
                    break;

                case CarpeState.recovering:
                        {
                        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, 0), recoverySpeed);
                        if (FastApproximatelyPrecise(transform.localPosition.z, 0))
                            {
                                carpeState = CarpeState.floating;
                            }
                        }
                        break;
        }

        
        if (invincibilityLeft > 0)
        {
            invincibilityLeft -= Time.deltaTime;
        }

        else
        {
            canBeHurt = true;
        }

        if (body.velocity != Vector3.zero)
        {
            body.velocity = Vector3.zero;
        }
    }

    public bool FastApproximately(float a, float b)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= .1f;
    }


    public bool FastApproximatelyPrecise(float a, float b)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= .025f;
    }

    public enum CarpeState
    {
        floating,
        startJump,
        endJump,
        stomp,
        hurt,
        recovering
    }
}
