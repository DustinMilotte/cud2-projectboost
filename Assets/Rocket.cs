using UnityEngine;

public class Rocket : MonoBehaviour {
    bool rotatingLeft = false;
    bool rotatingRight = false;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody> ();
        audioSource = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update () {
        Thrust ();
        Rotate ();
    }

    void OnCollisionEnter (Collision other) {
        switch (other.gameObject.tag) {
            case "Friendly":
                print ("Friendly");
                break;
            default:
                print ("Dead");
                break;
        }
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