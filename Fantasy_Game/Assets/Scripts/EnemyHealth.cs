using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private float dissapearSpeed = 2f;

    private AudioSource audio;
    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    private bool isAlive;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private bool dissapearEnemy = false;
    private int currentHealth;

    public bool IsAlive {
        get {
            return IsAlive;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>(); 
        audio = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        isAlive = true;
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (dissapearEnemy) {
            transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver) {
            if (other.tag == "PlayerWeapon") {
                takeHit();
                timer = 0f;
            }
        }
    }

    private void takeHit() {
        audio.PlayOneShot(audio.clip);

        if (currentHealth > 0) {
            anim.Play("Hurt");
            currentHealth -= 10;
        }

        //enemy has died
        if (currentHealth <= 0) {
            isAlive = false;
            KillEnemy();
        }
    }

    private void KillEnemy() {
        //no longer has the field to check for hits
        capsuleCollider.enabled = false;
        //no longer performs animations
        nav.enabled = false;
        //steps to the enemy death animation
        anim.SetTrigger("EnemyDie");
        //body is walk through after death
        rigidBody.isKinematic = true;
        StartCoroutine(RemoveEnemy());
    }

    IEnumerator RemoveEnemy()
    {
        //wait 4 seconds after enemy dies
        yield return new WaitForSeconds(4f);
        dissapearEnemy = true;
        //after 2 seconds of enemy falling through floor then destroy the object
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
