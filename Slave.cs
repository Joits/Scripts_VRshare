using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class Slave : Photon.MonoBehaviour { 

    // Use this for initialization
    void Start () {
        InputTracking.Recenter();
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(InputTracking.GetLocalRotation(VRNode.Head));
            //stream.SendNext(transform.rotation);
            Debug.Log(transform.rotation);
           // Debug.Log(InputTracking.GetLocalRotation(VRNode.Head));
        }
        //else
           // rigidbody.position = (Vector3)stream.ReceiveNext();
    }
    // Update is called once per frame
    void Update () {
	
	}
}
