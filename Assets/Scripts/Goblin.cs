using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
public class Goblin : MonoBehaviour {

    Animator anim;
    public Transform rewardPosition;
    public GameObject potion;
    public float speed;
    public int dir;
    float dirTimer = 1f;
    public int health;
    public GameObject deathParticle;
    bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = .2f;
    float specialTimer = .5f;


    bool shouldChange;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        canAttack = false;
        shouldChange = false;

    }

    // Update is called once per frame
    void Update()
    {
        specialTimer -= Time.deltaTime;
        if (specialTimer <= 0)
        {
            SpecialAttack();
            SpecialAttack();

            specialTimer = .5f;
        }
        dirTimer -= Time.deltaTime;
        if (dirTimer <= 0)
        {
            dirTimer = 1f;
            switch (dir)
            {
                case 1:
                    dir = 0;
                    break;
                case 2:
                    dir = 1;
                    break;
                case 3:
                    dir = 2;
                    break;
                case 0:
                    dir = 3;
                    break;

            }
        }
        Movement();

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = 2f;
            canAttack = true;
        }

        Attack();
        if (shouldChange)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0)
            {
                shouldChange = false;
                changeTimer = .2f;
            }
        }
    }
    void Attack()
    {
        if (!canAttack)
            return;
        canAttack = false;
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        if (dir == 0)
        {

            newProjectile.transform.Rotate(0, 0, 0);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        if (dir == 1)
        {
            newProjectile.transform.Rotate(0, 0, 90);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
        }
        if (dir == 2)
        {
            newProjectile.transform.Rotate(0, 0, 180);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);
        }
        if (dir == 3)
        {
            newProjectile.transform.Rotate(0, 0, -90);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
    }
    void Movement()
    {
        if (dir == 0)
        { transform.Translate(0, speed * Time.deltaTime, 0); anim.SetInteger("dir", dir); }
        else if (dir == 1)
        { transform.Translate(-speed * Time.deltaTime, 0, 0); anim.SetInteger("dir", dir); }
        else if (dir == 2)
        { transform.Translate(0, -speed * Time.deltaTime, 0); anim.SetInteger("dir", dir); }
        else if (dir == 3)
        { transform.Translate(speed * Time.deltaTime, 0, 0); anim.SetInteger("dir", dir); }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sword")
        {
            health--;
            col.gameObject.GetComponent<Sword>().CreatePaticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
            if (health <= 0)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Instantiate(potion, rewardPosition.position, rewardPosition.rotation);
                Thread.Sleep(1000);
                SceneManager.LoadScene(2);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ResetGame();
                Destroy(gameObject);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            health--;

            if (!col.gameObject.GetComponent<Player>().iniFrames)
            {
                col.gameObject.GetComponent<Player>().currentHealth--;
                col.gameObject.GetComponent<Player>().iniFrames = true;
            }
            if (health <= 0)
            {

                Instantiate(deathParticle, transform.position, transform.rotation);
                Thread.Sleep(1000);
                SceneManager.LoadScene(2);
                Destroy(gameObject);

            }

        }
        if (col.gameObject.tag == "Wall")
        {
            if (shouldChange)
                return;

            if (dir == 0)
                dir = 2;
            else if (dir == 1)
                dir = 3;
            else if (dir == 2)
                dir = 0;
            else if (dir == 3)
                dir = 1;
            shouldChange = true;

        }
    }

    void SpecialAttack()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        int randomDir = Random.Range(0, 4);
        switch (randomDir)
        {
            case 0:
                newProjectile.transform.Rotate(0, 0, -90);
                newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
                break;
            case 1:
                newProjectile.transform.Rotate(0, 0, 0);
                newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
                break;
            case 2:
                newProjectile.transform.Rotate(0, 0, 90);
                newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -thrustPower);
                break;
            case 3:
                newProjectile.transform.Rotate(0, 0, 180);
                newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -thrustPower);
                break;

        }
    }
}
