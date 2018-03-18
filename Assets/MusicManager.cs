using UnityEngine;

public class MusicManager : MonoBehaviour {
	private static MusicManager instance;

	private void Awake() {
		if (instance != null){
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
