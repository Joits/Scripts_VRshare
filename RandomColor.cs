using UnityEngine;
using System.Collections;

public class RandomColor : Photon.MonoBehaviour { //note the Photon.Monobehaviour!! give access to RPC over pun network

    // Use this for initialization
    private Renderer rend;
    [SerializeField]
    public GameObject Cube; //Gameobject which needs to be synchronized over PUN
                            // Update is called once per frame
    public float SpawnRate = 1F;
   
    Vector3 colorF = new Vector3(.1f, .55f, .8f);
    void Start()
    {

        rend = GetComponent<Renderer>();
    }
    void Update () {

        if (Input.GetMouseButtonUp(0))
        {

            colorF= new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
           // photonView.RPC("ChangeColorTo", PhotonTargets.Others, colorF);
            this.photonView.RPC("ChangeColorTo", PhotonTargets.All, colorF);
        }
        //if (photonView.isMine) {
        //    {
        //        float step = 0.2f;

        //        colorF.x += step;

        //        // Debug.Log("Pressed left click.");
        //        timestamp = Time.time + SpawnRate;
        //    }

    }
                   
    private void InputColorChange()
    {
        
    }

    [PunRPC] void ChangeColorTo(Vector3 color)
    {

        rend.material.color = new Color(color.x, color.y, color.z, 1f);
        if (photonView.isMine){
 
           // photonView.RPC("ChangeColorTo", PhotonTargets.Others, color);
        }//Debug.Log("Photon sync."); //use the photon view (assigned the gameobject) to change the color. 
        //photonView.RPC("CameraStream", PhotonTargets.All, affa)
        
    }

}
