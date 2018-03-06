using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    bool rotatingLeft = false;
    bool rotatingRight = false;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip newLevelSound;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (state == State.Alive) {
            RespondToThrust();
            RespondToRotate();
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
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence() {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(newLevelSound);
        Invoke("LoadNextLevel", 1f);
    }

    private void StartDeathSequence() {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadFirstLevel", 1f);
    }


    private void LoadFirstLevel() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene(1);
    }

    private void RespondToThrust() {
        if (Input.GetKey(KeyCode.Space)) {
            ApplyThrust();
        } else {
            audioSource.Stop();
        }
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotate() {
        rigidBody.freezeRotation = true;
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
        rigidBody.freezeRotation = false;
    }

}