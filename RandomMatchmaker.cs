using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.VR;
public class RandomMatchmaker : Photon.PunBehaviour //notice the Photon.PunBehavior making it able to access the OnjoinedRoom, OnPhotonRandomJoinFaile and OnJoinedLobby etc.
{

    private GameObject slave;
    private Camera mainCam;
    RenderTextureHandler RenderTexHandler;
	popupMessage warningMessage;
    private int index;
    public int playerIDTransfering;
    // private string roomName;

    private bool spawn = false;
    private int maxPlayer = 1;

    private Room[] game;
    private string roomName = "DEFAULT ROOM NAME";
    bool connecting = false;
    List<string> chatMessages;
    int maxChatMessages = 5;
    private string maxPlayerString = "2";
    public string Version = "Version 1";
    private Vector3 up;
    private Vector2 scrollPosition;
  //  private Transform panel;
    private List<GameObject> serverList;
    private GameObject scroll;
    private GameObject selectedObject;
    private Color unselectedColor;
    public GameObject ServerButton;

    Hashtable playerID = new Hashtable();

    void Start()
    {

        RenderTexHandler = FindObjectOfType(typeof(RenderTextureHandler)) as RenderTextureHandler;
		warningMessage = FindObjectOfType (typeof(popupMessage)) as popupMessage;
		mainCam = Camera.main;
        PhotonNetwork.ConnectUsingSettings(Version);
        if (PhotonNetwork.connected == false && connecting == false)
        {
            connecting = true;
            Connect();
        }
        if (PhotonNetwork.insideLobby == true)
        {
            GameObject nameField = GameObject.Find("Name_InputField"); //currently not doing anything
            PhotonNetwork.player.name = nameField.GetComponentInChildren<Text>().text;
        }
		GameObject canvasFade = GameObject.Find ("messageCanvas");
	//	CanvasGroup Fade = canvasFade.GetComponent<Canvas> ();
		//Fade.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void OnEnable()
    {
        if (serverList == null)
        {
         //   panel = transform.FindChild("Area/Panel");
            serverList = new List<GameObject>();
            unselectedColor = new Color(171 / 255.0f, 174 / 255.0f, 182 / 255.0f, 1);
        }
        InvokeRepeating("PopulateServerList", 3, 2);
    }

    public void OnDisable()
    {
        CancelInvoke();
    }



    public void PopulateServerList()
    {

     //   panel = transform.FindChild("Area/Panel");

        int i = 0;
        RoomInfo[] hostData = PhotonNetwork.GetRoomList();

      //  int selected = serverList.IndexOf(selectedObject);
        // print ( hostData.Length.ToString());

        //delete the serverlist
        for (int j = 0; j < serverList.Count; j++)
        {
            Destroy(serverList[j]);
        }
        serverList.Clear();

        //generate a new list of buttons with available servers
        if (null != hostData)
        {
            for (i = 0; i < hostData.Length; i++)
            {
                if (!hostData[i].open)
                    continue;

                GameObject Server = (GameObject)Instantiate(ServerButton, ServerButton.transform.parent);
                serverList.Add(Server);
                Server.SetActive(true);
                Server.transform.SetParent(ServerButton.transform.parent);
                // go.transform.SetParent(Button_Template.transform.parent);

                Server.transform.FindChild("ServerText").GetComponent<Text>().text = hostData[i].name;
                // text.transform.FindChild("PlayerText").GetComponent<Text>().text = hostData[i].playerCount + "/" + hostData[i].maxPlayers;
                //text.transform.FindChild("MapText").GetComponent<Text>().text = hostData[i].customProperties["map"].ToString();
                //text.transform.FindChild("GMText").GetComponent<Text>().text = hostData[i].customProperties["gm"].ToString();
                Transform t = Server.GetComponent<Transform>().transform;
                t.Translate(new Vector3(0, 0, i * 20)); //=  i*20; //stupid, make it react to a scroll list style TO BE DONE (- text.transform.lossyScale.y)
            }
        }

    }

    public void CreateSession()
    {
        GameObject go = GameObject.Find("serverTxtField");
        roomName = go.GetComponent<Text>().text;

        if (roomName != "" && maxPlayer > 0)
        {
            //  PhotonNetwork.CreateRoom(roomName, true, true, maxPlayer);

            RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 5 };
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
            //    print(PhotonNetwork.GetRoomList());
        }

    }
    public void Connect()
    {

    }


    public override void OnJoinedLobby() // try to join random room
    {
        Debug.Log("In on joined lobby");
        GameObject connectionStatus = GameObject.Find("ConnectedStatus");
        //connectionStatus.GetComponentInChildren<Text>().text = PhotonNetwork.connectionStateDetailed.ToString();
        if (PhotonNetwork.connected)
        {
            connectionStatus.GetComponent<Image>().color = new Color32(0, 255, 0, 128);
            connectionStatus.GetComponentInChildren<Text>().text = "Connected, ready to join.";
        }
        else
        {
			connectionStatus.GetComponent<Image>().color = new Color32(245, 54, 54, 128);
            connectionStatus.GetComponentInChildren<Text>().text = "Not connected, check internet connection and try again.";
        }
        //  PhotonNetwork.JoinRandomRoom();
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        GameObject connectionStatus = GameObject.Find("ConnectedStatus");
        connectionStatus.GetComponentInChildren<Text>().text = "Can't connect, check internet connection and try again.";
    }
    void OnPhotonRandomJoinFailed() //no room? create one!!
    {

    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (PhotonNetwork.isMasterClient)
        {
            // RenderTexHandler.resetRenderTex();
            RenderTexHandler.resetRenderTexCall();
        }

    }

    public override void OnJoinedRoom()
    {
        //print("in room");
        //   OnDisable(); // disable the invoke so it doesnt run in the background
        

		mainCam.cullingMask = -1;
        if (PhotonNetwork.playerList.Length == 1)
        {
            //  Debug.Log("one player");
        }

        if (PhotonNetwork.playerList.Length > 1)
        {
            // Debug.Log("two players");
            StartCoroutine(delayConnect());

        }
        //go = GameObject.Find("Plane");
        if (!PhotonNetwork.isMasterClient)
        {
            // Debug.Log("not master client, reset cam..");
            //  mainCam.enabled = true;
            Vector3 reset = new Vector3(0, 0, 0);
            mainCam.transform.position = reset;
            mainCam.orthographic = false;
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));

        }
        else
        {
            Vector3 ortho = new Vector3(1010, 0, 0);
            mainCam.transform.position = ortho;
            mainCam.orthographic = true;
            mainCam.orthographicSize = 148;
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("SphereLayer"));
            VRSettings.enabled = false;
            // VRSettings.enabled = true;

        }

    }
    private IEnumerator delayConnect()
    {

        yield return new WaitForSeconds(0.5f); //we need a small delay else the slaveprefab wouldnt exist on both clients.
        //mainCam = Camera.main;
        //based on how many people and when you joined, create a prefab which will handle data transfer.
        if (GameObject.Find("Slave_preFab2(Clone)") == null)
        {
            slave = PhotonNetwork.Instantiate("Slave_preFab2", new Vector3(0, 0, 0), Quaternion.identity, 0);
            index = 0;
            playerID["ID"] = index;
            PhotonNetwork.player.SetCustomProperties(playerID);
        }
        else if (GameObject.Find("Slave_preFab3(Clone)") == null)
        {
            slave = PhotonNetwork.Instantiate("Slave_preFab3", new Vector3(0, 0, 0), Quaternion.identity, 0);
            index = 1;
            playerID["ID"] = index;
            PhotonNetwork.player.SetCustomProperties(playerID);
        }
        else if (GameObject.Find("Slave_preFab4(Clone)") == null)
        {
            slave = PhotonNetwork.Instantiate("Slave_preFab4", new Vector3(0, 0, 0), Quaternion.identity, 0);
            index = 2;
            playerID["ID"] = index;
            PhotonNetwork.player.SetCustomProperties(playerID);
        }
        else {
            slave = PhotonNetwork.Instantiate("Slave_preFab5", new Vector3(0, 0, 0), Quaternion.identity, 0);
            index = 3;
            playerID["ID"] = index;

            PhotonNetwork.player.SetCustomProperties(playerID);

        }

        slave.GetComponentInChildren<Camera>().enabled = true; 
        slave.GetComponentInChildren<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("LoginGUI"));
        mainCam.enabled = false;
        photonView.RPC("okSignal", PhotonTargets.MasterClient, slave.name, index); //send signal to master about who joins
        StopCoroutine(delayConnect());
 
    }

    public void returnPlayerID()
    {
       if(PhotonNetwork.insideLobby == false) 
        photonView.RPC("returnID", PhotonTargets.All, (int)(playerID["ID"]));
        // print(PhotonNetwork.player.customProperties["ID"].ToString());
    }

    [PunRPC]
    public void returnID(int ID)
    {
        playerIDTransfering = ID; //set player ID to a variable which is public
    }

    [PunRPC]
    public void okSignal(string signal, int ind)
    {
        RenderTexHandler.createRenderMat(signal, ind); //create a new render material based on who joins
    }

	public void OnDisconnectedFromPhoton (){
		Debug.Log("Lost connection from photon, reconnect");
		InvokeRepeating("PopulateServerList", 3, 2); //doesnt actually work!?

		mainCam.enabled = true;
		warningMessage.popUpMessage("Lost connection to the server... Please try to reconnect to the Internet.");

	}


	// The master has disconnected, leave room and return to server list.
	void OnMasterClientSwitched( PhotonPlayer newMaster )
	{
		Debug.Log("The old masterclient left, we have a new masterclient: " + newMaster);
		InvokeRepeating("PopulateServerList", 3, 2); 
		mainCam.enabled = true;
		warningMessage.popUpMessage("Session leader disconnected.. please wait and reconnect to a new server");

		PhotonNetwork.LeaveRoom (); 

	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //need this in order to avoid an error.
    }




}
