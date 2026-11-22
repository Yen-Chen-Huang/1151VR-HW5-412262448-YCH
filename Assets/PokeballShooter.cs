using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeballController : MonoBehaviour
{
    [Header("力道 (Vector3)")]
    public Vector3 shootForce = new Vector3(180f, 140f, 0f);

    [Header("收服特效與目標")]
    public ParticleSystem captureEffectPrefab;
    public bool hideHitTarget = true;

    private Rigidbody rb;
    private Quaternion handInitRot;
    private bool hasCaptured = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();


        Shoot(shootForce);
    }

    public void Shoot(Vector3 dir)
    {

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(dir);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (hasCaptured) return;

        if (other.gameObject.GetComponent<BoxCollider>() != null)
        {
            hasCaptured = true;

            Vector3 spawnPos = other.contacts.Length > 0
                ? other.contacts[0].point
                : other.transform.position;

            if (captureEffectPrefab != null)
            {
                ParticleSystem vfx = Instantiate(captureEffectPrefab, spawnPos, Quaternion.identity);
                vfx.Play();
                Destroy(vfx.gameObject, 3f);
            }

            if (hideHitTarget)
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}