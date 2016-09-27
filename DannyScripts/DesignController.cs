//Daniel Todorov - saso3d.com

using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class DesignController : MonoBehaviour {

	int totalNrOfTextures;
	public Texture[] textureArray0 = new Texture[4];
	public Texture[] textureArray1 = new Texture[4];
	public Texture[] textureArray2 = new Texture[4];
	public Texture[] textureArray3 = new Texture[4];
	private int[] currentTexture = new int[4];
	int currentLayerToAdjust = 0;
	ButtonDesign lastUsedButton;


	[SerializeField]
	private VRInput m_VRInput; 

	private void OnEnable()
	{
		m_VRInput.OnSwipe += HandleSwipe;
	}

	private void OnDisable()
	{
		m_VRInput.OnSwipe -= HandleSwipe;
	}


	//Handling VR input swipes and actions uppon swipes
	public void HandleSwipe(VRInput.SwipeDirection swipeDirection)
	{

		switch (swipeDirection)
		{
		case VRInput.SwipeDirection.LEFT:
			currentTexture [0] = (currentTexture [0] - 1);
			switch (currentLayerToAdjust) {

			case 0:
				this.GetComponent<Renderer> ().material.SetTexture("_MainTex", textureArray0 [Mathf.Abs (currentTexture [0] % totalNrOfTextures)]);
				break;
			case 1:
				this.GetComponent<Renderer> ().material.SetTexture("_FirstLayer", textureArray1 [Mathf.Abs (currentTexture [0] % totalNrOfTextures)]);
				break;
			case 2:
				this.GetComponent<Renderer> ().material.SetTexture("_SecondLayer", textureArray2 [Mathf.Abs (currentTexture [0] % totalNrOfTextures)]);
				break;
			case 3:
				this.GetComponent<Renderer> ().material.SetTexture("_ThirdLayer", textureArray3 [Mathf.Abs (currentTexture [0] % totalNrOfTextures)]);
				break;
			}
			break;
		case VRInput.SwipeDirection.RIGHT:
			currentTexture[0] = (currentTexture[0] + 1);
			switch (currentLayerToAdjust){

			case 0:
				this.GetComponent<Renderer> ().material.SetTexture("_MainTex", textureArray0 [Mathf.Abs (currentTexture [0] % totalNrOfTextures)]);
				break;
			case 1:
				this.GetComponent<Renderer> ().material.SetTexture("_FirstLayer", textureArray1 [Mathf.Abs (currentTexture [0] % totalNrOfTextures)]);
				break;
			case 2:
				this.GetComponent<Renderer> ().material.SetTexture("_SecondLayer", textureArray2 [Mathf.Abs (currentTexture [0] % totalNrOfTextures)]);
				break;
			case 3:
				this.GetComponent<Renderer> ().material.SetTexture("_ThirdLayer", textureArray3 [Mathf.Abs (currentTexture [0] % totalNrOfTextures)]);
				break;
			}
			break;
		}
	}

	// Use this for initialization
	void Start () {
		totalNrOfTextures = textureArray0.Length;
		currentTexture[0] = totalNrOfTextures * 19;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool changeChannel(int i, ButtonDesign q){

		currentLayerToAdjust = i;

		if (lastUsedButton != null) {
			lastUsedButton.setToInactive();
			lastUsedButton = q;
		} else {
			lastUsedButton = q;
		}
		return true;
	}



}
