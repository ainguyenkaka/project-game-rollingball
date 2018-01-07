using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeIconByClick : MonoBehaviour
{

	public Sprite icon1, icon2;

	private Image buttonImage;
	private Button button;

	// Use this for initialization
	void Start ()
	{
		buttonImage = GetComponent<Image> ();
		button = GetComponent<Button> ();
		button.onClick.AddListener (ButtonClick);

	
		EventSystem.current.SetSelectedGameObject (button.gameObject);
	}


	void ButtonClick ()
	{
		if (buttonImage.sprite.Equals (icon1)) {
			buttonImage.sprite = icon2;
		} else {
			buttonImage.sprite = icon1;
		}
	}


}
