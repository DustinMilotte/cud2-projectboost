using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    bool rotatingLeft = false;
    bool rotatingRight = false;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Alive, Dead, Transcending}
    State state = State.Alive;

    void Start () {
        rigidBody = GetComponent<Rigidbody> ();
        audioSource = GetComponent<AudioSource> ();
    }

    void Update () {
        if(state == State.Alive){
            Thrust ();
            Rotate ();
        }
    }
    
    void OnCollisionEnter (Collision other) {
        if(state != State.Alive) { return; }
        switch (other.gameObject.tag) {
            case "Friendly":
                print ("Friendly");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f);
                break;
            default:
                state = State.Transcending;
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadFirstLevel(){
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel(){
        SceneManager.LoadScene(1);
    }

    private void Thrust () {
        if (Input.GetKey (KeyCode.Space)) {
            rigidBody.AddRelativeForce (Vector3.up * mainThrust);
            if (!audioSource.isPlaying) {
                audioSource.Play ();
            }
        } else {
            audioSource.Stop ();
        }
    }

    private void Rotate () {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey (KeyCode.A) && !rotatingRight) {
            transform.Rotate (Vector3.forward * rotationThisFrame);
            rotatingLeft = true;
        }
        if (Input.GetKeyUp (KeyCode.A)) {
            rotatingLeft = false;
        }
        if (Input.GetKey (KeyCode.D) && !rotatingLeft) {
            transform.Rotate (-Vector3.forward * rotationThisFrame);
            rotatingRight = true;
        }
        if (Input.GetKeyUp (KeyCode.D)) {
            rotatingRight = false;
        }
        rigidBody.freezeRotation = false;
    }

}