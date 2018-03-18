using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour {
	void Start (){
		
	}
	
	void Update (){
		if(Input.anyKey){
			SceneManager.LoadScene(1);
		}
	}
}
