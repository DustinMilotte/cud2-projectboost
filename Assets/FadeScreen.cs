using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour {
	Image image;

	private void Start() {
		image = GetComponent<Image>();

		image.CrossFadeAlpha(0, 2f, true);
	}

}
