using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

	private static ScoreKeeper instance;
	public static int score;
	public Text playerCashText;

	private void Awake() {
		if(instance != null && instance != this){
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
	}

	private void Start() {
		print("score= " + score);
		playerCashText.text = score.ToString();
	}

	private void Update() {
		playerCashText.text = score.ToString();
	}
}
