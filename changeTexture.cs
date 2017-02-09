using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections.Generic;

//this script handles the interaction from the touchpad of the GearVR to shuffle through textures specified in the array.
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
		[Header("If there are any channels associated with the main texture load them here:")]
		[SerializeField] public Texture[] channels;
	}
	//public rowData[] columns = new rowData[8];

	public rowData[] imagesToLoad;
	public Texture[] textures; //texture array. set this to the amount of pictures in the inspector and load them in the different slots.
    private Renderer rend;

    private int index;


    [PunRPC]
    private void OnEnable()
    {
        m_VRInput.OnSwipe += HandleSwipe;
      
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
                    index = textures.Length - 1;
                photonView.RPC("changeTextureRPC", PhotonTargets.AllBuffered, index);
                //set texture to current index number
                break;
            case VRInput.SwipeDirection.RIGHT:
                index++;
                if (index > textures.Length - 1) //if reaching the end of the array, set to beginning
                    index = 0;
                photonView.RPC("changeTextureRPC", PhotonTargets.AllBuffered, index); //set texture to current index number
                break;
        }
    }

    public void masterChangeTextureIndex(int newIndex) //Receive the new index from the button script and send RPC based on incomming image list number
    {
        photonView.RPC("changeTextureRPC", PhotonTargets.AllBuffered, newIndex);
    }


    [PunRPC]
    public void changeTextureRPC(int newIndex)
    {
       //send information about the new texture index to all cllients
     // rend.material.mainTexture = textures[newIndex];
		rend.material.mainTexture = imagesToLoad[newIndex].mainTex;
    
    }


    void Start()
    {
        //assign renderer
        
        rend = GetComponent<Renderer>();

        //index for shuffling trhough the amount of textures
        index = 0;

        //rend.material.mainTexture = textures[0];
   
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //need this in order to avoid an error.
    }
}

