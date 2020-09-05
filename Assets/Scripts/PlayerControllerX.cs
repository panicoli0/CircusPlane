using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    public bool isOnGround = true;

    public float speed = 5.0f;
    public float turnSpeed = 25.0f;
    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;
    public float horizontalInput;
    public float rotationSpeed;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    // El player se mueve verticalmente con la barra y horizontalmente con las rows
    void Update()
    {
        //Player move forward at constant speed 
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // get the user's vertical input
        float horizontalInput = Input.GetAxis("Horizontal");
        //Mover al player de izq a der
        playerRb.AddForce(Vector3.right * speed * horizontalInput);

        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver)
        {
            transform.Rotate(Vector3.left * turnSpeed * rotationSpeed * Time.deltaTime);
            playerRb.AddForce(Vector3.up * floatForce * 3);
            
            isOnGround = false;
        } else {
            playerRb.AddForce(Vector3.down * floatForce);
            transform.Rotate(Vector3.right * turnSpeed * rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            isOnGround = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            gameOver = true;
            Debug.Log("Game Over!");
        }
    }

}
