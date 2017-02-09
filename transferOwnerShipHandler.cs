using UnityEngine;
using System.Collections;

public class transferOwnerShipHandler : MonoBehaviour {

    public bool TransferOwnershipOnRequest = true;

    public void OnOwnershipRequest(object[] viewAndPlayer)
    {
        PhotonView view = viewAndPlayer[0] as PhotonView;
        PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;

        Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + view + ".");
        if (this.TransferOwnershipOnRequest)
        {
            view.TransferOwnership(requestingPlayer.ID);
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
