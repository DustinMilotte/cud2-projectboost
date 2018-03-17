using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {

	void Start (){
		
	}
	
	void Update (){
		if(Input.anyKey){
			SceneManager.LoadScene(1);
		}
	}
}
