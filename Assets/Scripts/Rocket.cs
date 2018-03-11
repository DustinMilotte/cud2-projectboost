using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip newLevelSound;

    [SerializeField] ParticleSystem mainEngineParticlesBack;
    [SerializeField] ParticleSystem mainEngineParticlesFront;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    bool rotatingLeft = false;
    bool rotatingRight = false;
    bool collisionsOn = true;
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    int timer = 30;
    public Text countdownText;


    private ScoreKeeper scoreKeeper;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("Countdown");
    }

    void Update() {
        if (state == State.Alive) {
            RespondToThrust();
            RespondToRotate();
        }
        if (Debug.isDebugBuild) {
            RespondToDebugKeys();
        }
        
        countdownText.text = "due in: " + timer;
        
        if (timer <= 0 && state == State.Alive){
            StopCoroutine("Countdown");
            StartDeathSequence();
            state = State.Transcending;
        }
    }

    private void RespondToThrust() {
        if (Input.GetKey(KeyCode.Space)) {
            ApplyThrust();
        } else {
            StopApplyingThrust();
        }
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticlesFront.Play();
        mainEngineParticlesBack.Play();
    }

    private void StopApplyingThrust() {
        audioSource.Stop();
        mainEngineParticlesBack.Stop();
        mainEngineParticlesFront.Stop();
    }

    private void RespondToRotate() {
        rigidBody.angularVelocity = Vector3.zero;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && !rotatingRight) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
            rotatingLeft = true;
        }
        if (Input.GetKeyUp(KeyCode.A)) {
            rotatingLeft = false;
        }
        if (Input.GetKey(KeyCode.D) && !rotatingLeft) {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
            rotatingRight = true;
        }
        if (Input.GetKeyUp(KeyCode.D)) {
            rotatingRight = false;
        }
    }

    private void RespondToDebugKeys() {
        if (Input.GetKey(KeyCode.L)) {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            collisionsOn = !collisionsOn;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (state != State.Alive) { return; }
        switch (other.gameObject.tag) {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                if (collisionsOn) {
                    StartDeathSequence();
                }
                break;
        }
    }

    private void StartSuccessSequence() {
        state = State.Transcending;
        StopCoroutine("Countdown");
        ScoreKeeper.score += timer;
        audioSource.Stop();
        audioSource.PlayOneShot(newLevelSound);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence() {
        state = State.Dying;
        ScoreKeeper.score = 0;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadFirstLevel() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel() {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene < (SceneManager.sceneCountInBuildSettings) - 1) {
            SceneManager.LoadScene(currentScene + 1);
        } else {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator Countdown(){
        while(true){
            yield return new WaitForSeconds(1);
            timer--;
        }
    }
}