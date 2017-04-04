using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections.Generic;
using UnityEngine.UI;

//this script handles the interaction from the touchpad of the GearVR to shuffle through textures specified in the array.
//also handles multilayered textures and the different channels
//Will also handle creation of shaders for multilayer texture control.

public class changeTexture : Photon.MonoBehaviour
{
    [SerializeField]
    private VRInput m_VRInput; //the VR input which is the camera
    [SerializeField]
    private GameObject m_projectionSphere; //the game object which is the interactive part and need textures to be changed.
   	


	[System.Serializable]
	public struct rowData
	{
		[Header("Base image i.e. the spherical image, load here.")]
		public Texture mainTex;
		[Header("If there are any sub-channels associated with the main texture load them here:")]
		[SerializeField] public Texture[] channels;
	}

	public rowData[] imagesToLoad; //texture array. set this to the amount of pictures in the inspector and load them in the different slots, this also contains the ability for sub-channels. 

	public Texture[] textures; //LEGACY texture array. set this to the amount of pictures in the inspector and load them in the different slots. 
    private Renderer rend;
	[SerializeField]
	public Shader interActiveShader;
    private int index; //controls the place in the texture array
	private Material[] mats;
	private string[] chans;
	private List<Material> matList;
	private List<string> chanList;
	private Slider[] sliders = new Slider[8];

	sliderChange refSlider;

    [PunRPC]
    private void OnEnable()
    {
    //    m_VRInput.OnSwipe += HandleSwipe;
      
    }

    [PunRPC]
    private void OnDisable()
    {
        m_VRInput.OnSwipe -= HandleSwipe;
       
    }




    public List<string> returnImageList() //list of textures public available by request :D
    {
        List<string> imageList = new List<string>();

//        foreach (Texture txt in textures)
//        {
//            imageList.Add(txt.name);
//        }
//		foreach (Texture txt in imagesToLoad)
//		{
//			imageList.Add(txt.name);
//		}

		for (int i = 0; i < imagesToLoad.Length; i++) 
		{
			imageList.Add(imagesToLoad[i].mainTex.name);
		}
        return imageList;

    }

    public void userControl(bool userCtrl)
    {
        if (userCtrl)
            photonView.RPC("OnEnable", PhotonTargets.AllBuffered);
        else
            photonView.RPC("OnDisable", PhotonTargets.AllBuffered);
    }
    


    public void HandleSwipe(VRInput.SwipeDirection swipeDirection)
    {
        if (textures.Length == 0)
        {
            Debug.Log("no textures assigned to the sphere");//if there are no textures assigned, break.
            return;
            
        }
		if (!PhotonNetwork.isMasterClient){
      	  switch (swipeDirection)
       	 {

            case VRInput.SwipeDirection.NONE:
                break;
            case VRInput.SwipeDirection.UP:
                break;
            case VRInput.SwipeDirection.DOWN:
                break;
            case VRInput.SwipeDirection.LEFT:
                index--;
                if (index < 0) //go the end of the array if the beginning is reached
				index = imagesToLoad.Length - 1;
                photonView.RPC("changeTextureRPC", PhotonTargets.AllBuffered, index);
                //set texture to current index number
                break;
            case VRInput.SwipeDirection.RIGHT:
                index++;
			if (index > imagesToLoad.Length - 1) //if reaching the end of the array, set to beginning
                    index = 0;
                photonView.RPC("changeTextureRPC", PhotonTargets.AllBuffered, index); //set texture to current index number
                break;
			}
        }
    }

    public void masterChangeTextureIndex(int newIndex) //Receive the new index from the button script and send RPC based on incomming image list number
    {
        photonView.RPC("changeTextureRPC", PhotonTargets.AllBuffered, newIndex);
		index = newIndex;
    }


    [PunRPC]
    public void changeTextureRPC(int newIndex)
    {
       //send information about the new texture index to all cllients
     // rend.material.mainTexture = textures[newIndex]; //lecacy

	//	rend.material.mainTexture = imagesToLoad[newIndex].mainTex;
	
		loadMaterial (newIndex);
    }


    void Start()
    {
        //assign renderer
		matList = new List<Material>();
		chanList = new List<string> ();
        rend = GetComponent<Renderer>();
		refSlider = FindObjectOfType (typeof(sliderChange)) as sliderChange;
			
        //index for shuffling trhough the amount of textures
        index = 0;
		createShaders ();

		for (int i = 0; i < 8; ++i) {
			GameObject go = GameObject.Find ("Channel " + i);
		//	print (go);
			sliders[i] = go.GetComponent<Slider>();
		}
	
        //rend.material.mainTexture = textures[0];
   
    }

	void Update()
	{
		if (Input.GetKey("up")) 
		{
			for (int i = 0; i < mats.Length; i++)
			print (mats [i].name);
		}



	
	}


	public void sliderChange(float input, string slider)
	{
		//which slider is it?
		//string sliderID = refSlider.onValueChangedName();

		char sliderIDtrimmed = slider[slider.Length-1];



		//mats[index].SetFloat("_Ambient",0.1f);

		photonView.RPC("sliderChangeRPC", PhotonTargets.All, input, sliderIDtrimmed.ToString());
		//print ("changing " + mats[index] + " channel _Blend "+ sliderIDtrimmed.ToString() + " with a value of: "+ input);
	
	}

	[PunRPC]
	public void sliderChangeRPC (float input, string slider){
		this.rend.material.SetFloat ("_Blend" + slider, input);
		//mats[index].SetFloat("_Blend" + slider, input);
	}


	private void loadMaterial (int index)
	{
		Material chosenMat = mats [index];
		this.rend.material = chosenMat;


	
		if (PhotonNetwork.isMasterClient) 
		{
			for (int i = 0; i<8;i++){
				sliders [i].GetComponentInChildren<Text> ().text = "None";
				sliders [i].interactable = false;
			}
			if (chosenMat.name == "interactiveMat " + index) {
				//for (int i = 0; i < imagesToLoad.Length; i++) {

					for (int j = 0; j <= imagesToLoad [index].channels.Length-1; j++) {

					sliders [j].GetComponentInChildren<Text> ().text = imagesToLoad [index].channels [j].name;
					sliders [j].interactable = true;
					}
				//}
			} 
			else
			{

			}
		
		}
	
	}

	private void createShaders ()
	{
		//Shader interactiveShader = Shader.Find ("interActiveLightingShader");
		//Material[] sharedMats= this.rend.sharedMaterials;
		for (int i = 0; i < imagesToLoad.Length; i++) 
		{
			if (imagesToLoad [i].channels.Length == 0) //if there are no channels
			{
				Material material = new Material(Shader.Find("Unlit/Texture"));
				material.name = "flatMat " + i;
				material.SetTexture("_MainTex", imagesToLoad[i].mainTex);

				material.mainTextureScale = new Vector2 (-1, 1); //need to invert the U channel in order to mirror the image UV = (-1,1)
				matList.Insert(i, material); //need to store the materials created in the shared material 
				//break;
			}
			else
			{
				//print (imagesToLoad [i].channels.Length);
				Material material = new Material (interActiveShader);
				material.name = "interactiveMat " + i;
				material.SetTexture("_AmbientLight", imagesToLoad[i].mainTex);

				for (int j = 0; j < imagesToLoad[i].channels.Length; j++)  //maybe out of index error subtract 1 from length or channels array
				{
					if (imagesToLoad[i].channels[j] == null) break;

					material.SetTexture ("_Blend"+j+"Light", imagesToLoad[i].channels[j]);
					chanList.Add (imagesToLoad [i].channels [j].name);


				}
				material.mainTextureScale = new Vector2 (-1, 1); //need to invert the U channel in order to mirror the image UV = (-1,1)
				matList.Insert(i, material);
			}
		//	Debug.Log ("i is: " + i);
		///	Debug.Log (": " + matList [i].name);
		}
		mats = matList.ToArray ();
		chans = chanList.ToArray ();
	}
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //need this in order to avoid an error.
    }
}

