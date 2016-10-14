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
    private Transform panel;
    private List<GameObject> serverList;
    private GameObject scroll;
    private GameObject selectedObject;
    private Color unselectedColor;
    public GameObject ServerButton;

    Hashtable playerID = new Hashtable();

    void Start()
    {
        roomName = "lol";
        RenderTexHandler = FindObjectOfType(typeof(RenderTextureHandler)) as RenderTextureHandler;
        PhotonNetwork.ConnectUsingSettings(Version);
        if (PhotonNetwork.connected == false && connecting == false)
        {
            connecting = true;
            Connect();
        }
        if (PhotonNetwork.insideLobby == true)
        {
            GameObject nameField = GameObject.Find("Name_InputField");
           PhotonNetwork.player.name = nameField.GetComponentInChildren<Text>().text;
        }
                
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("----------------------");
        //foreach (PhotonPlayer other in PhotonNetwork.playerList)
        //{

        //    Debug.Log(other.customProperties["ID"].ToString());
        //    int id = (int)other.customProperties["ID"];
        //    Debug.Log(id);
        //    Debug.Log(other.GetNext());
        //    playerText[PhotonNetwork.playerList.Length - 2 + playerIndex].text = ("User ID " + other + " connected! | " + other.customProperties["Ping"].ToString());

        //    GUILayout.Label("Player number " + pl + " connected! | " + pl.customProperties["Ping"].ToString());

        //}

        //PlayerCustomProps["Ping"] = PhotonNetwork.GetPing();
        //print(PhotonNetwork.GetRoomList());


        //PhotonNetwork.player.SetCustomProperties(PlayerCustomProps);
        //if (PhotonNetwork.inRoom == true)
        //{
        //    // print("in room");
        //    foreach (RoomInfo game in PhotonNetwork.GetRoomList())
        //    {

        //        print(game.maxPlayers.ToString());
        //        //if (GUILayout.Button("Join Session"))
        //        //{
        //        //    PhotonNetwork.JoinRoom(game.name);
        //        //}
        //    }
        //}

    }


    public void OnEnable()
    {
        if (serverList == null)
        {
            panel = transform.FindChild("Area/Panel");
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

        panel = transform.FindChild("Area/Panel");
  
        int i = 0;
        RoomInfo[] hostData = PhotonNetwork.GetRoomList();

        int selected = serverList.IndexOf(selectedObject);
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
                t.Translate(new Vector3(0,0,i*20)); //=  i*20; //stupid, make it react to a scroll list style TO BE DONE (- text.transform.lossyScale.y)
            }
        }
        if ((i * -25) < -290)
        {
         //   panel.GetComponent<RectTransform>().sizeDelta = new Vector2(400, (i * 25) + 30);
         //   scroll.SetActive(true);
        }
        else
        {
          //  panel.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 320);
        //    scroll.SetActive(false);
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
    //void OnGUI()
    //{
    //    GUI.color = Color.grey;
    //    if (PhotonNetwork.connected == false && connecting == false)
    //    {


    //        connecting = true;
    //        Connect();


    //    }
    //    if (PhotonNetwork.connected == true && connecting == false)
    //    {
    //        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
    //        GUILayout.BeginVertical();
    //        GUILayout.FlexibleSpace();

    //        foreach (string msg in chatMessages)
    //        {
    //            GUILayout.Label(msg);
    //        }

    //        GUILayout.EndVertical();
    //        GUILayout.EndArea();

    //    }

    //    if (PhotonNetwork.insideLobby == true)
    //    {

    //        GUI.Box(new Rect(Screen.width / 2.5f, Screen.height / 3f, 400, 550), "");
    //        GUI.color = Color.white;
    //        GUILayout.BeginArea(new Rect(Screen.width / 2.5f, Screen.height / 3, 400, 500));
    //        GUI.color = Color.cyan;
    //        GUILayout.Box("Lobby");
    //        GUI.color = Color.white;

    //        GUILayout.Label("Username: ");
    //        PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);

    //        GUILayout.Label("Session Name:");
    //        roomName = GUILayout.TextField(roomName);
    //        GUILayout.Label("Max amount of players 1 - 20:");
    //        maxPlayerString = GUILayout.TextField(maxPlayerString, 2);
    //        if (maxPlayerString != "")
    //        {

    //            maxPlayer = int.Parse(maxPlayerString);

    //            if (maxPlayer > 20) maxPlayer = 20;
    //            if (maxPlayer == 0) maxPlayer = 1;
    //        }
    //        else
    //        {
    //            maxPlayer = 1;
    //        }

    //        if (GUILayout.Button("Create Room "))
    //        {
    //            if (roomName != "" && maxPlayer > 0)
    //            {
    //                //  PhotonNetwork.CreateRoom(roomName, true, true, maxPlayer);

    //                RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 5 };
    //                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    //            }
    //        }

    //        GUILayout.Space(20);
    //        GUI.color = Color.yellow;
    //        GUILayout.Box("Sessions Open");
    //        GUI.color = Color.red;
    //        GUILayout.Space(20);

    //        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(400), GUILayout.Height(300));


    //        foreach (RoomInfo game in PhotonNetwork.GetRoomList())
    //        {
    //            GUI.color = Color.green;
    //            GUILayout.Box(game.name + " " + game.playerCount + "/" + game.maxPlayers + " " + game.visible);
    //            if (GUILayout.Button("Join Session"))
    //            {
    //                PhotonNetwork.JoinRoom(game.name);
    //            }
    //        }
    //        GUILayout.EndScrollView();
    //        GUILayout.EndArea();
    //    }
    //}

    public override void OnJoinedLobby() // try to join random room
    {
        Debug.Log("In on joined lobby");
        //  PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed() //no room? create one!!
    {
        //   Debug.Log("Can't join random room!");
        //RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 5 };
        //PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        //    PhotonNetwork.CreateRoom(null); //cant have an empty call (no overloaded functions), use null instead.
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (PhotonNetwork.isMasterClient)
        {
            // RenderTexHandler.resetRenderTex();
            RenderTexHandler.resetRenderTexCall();
        }

        // img[PhotonNetwork.playerList.Length - 1].texture = noUserConnectedTex;
    }

    public override void OnJoinedRoom()
    {
        //print("in room");
     //   OnDisable(); // disable the invoke so it doesnt run in the background
        mainCam = Camera.main;
        
        if (PhotonNetwork.playerList.Length == 1)
        {
            //  Debug.Log("one player");
        }

        if (PhotonNetwork.playerList.Length > 1)
        {
           // Debug.Log("two players");
            StartCoroutine(delay());
           
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
    private IEnumerator delay()
    {

        yield return new WaitForSeconds(0.5f); //we need a small delay else the slaveprefab wouldnt exist on both clients.
        

        if (PhotonNetwork.isMasterClient)
        {
            index = 999;
            playerID["ID"] = index;
            PhotonNetwork.player.SetCustomProperties(playerID);
        }
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
        //slave = PhotonNetwork.Instantiate("Slave_preFab2", new Vector3(0, 0, 0), Quaternion.identity, 0);
        Debug.Log(index);
        slave.GetComponentInChildren<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("LoginGUI"));
        photonView.RPC("okSignal", PhotonTargets.MasterClient, slave.name, index); //send signal to master about who joins
        // print(Time.time);
    }

    public void returnPlayerID()
    {
       photonView.RPC("returnID", PhotonTargets.All, (int)(playerID["ID"]));
      // print(PhotonNetwork.player.customProperties["ID"].ToString());
    }

    [PunRPC] 
    public void returnID(int ID)
    {
        playerIDTransfering = ID;
       // if (PhotonNetwork.isMasterClient)
            print(ID);
       // playerIDTransfering = PhotonNetwork.player.customProperties["ID"].ToString();
        //print(PhotonNetwork.player.customProperties["ID"].ToString());
    }

    [PunRPC]
    public void okSignal(string signal, int ind)
    {
        //Debug.Log("in RPC okSignal");
        //string preFabCheck = ("Slave_preFab" + PhotonNetwork.playerList.Length +"(Clone)");
        //GameObject slavePreFab = GameObject.Find(preFabCheck);

        //if (slavePreFab != null)
        //{
        //    int k = 2;
        //    while (slavePreFab != null | k <= 4)
        //    {
        //        preFabCheck = ("Slave_preFab" + k + "(Clone)");
        //        slavePreFab = GameObject.Find(preFabCheck);
        //        if (slavePreFab != null)
        //        {
        //            break;
        //        }
        //        k++;
        //        Debug.Log(slavePreFab);
        //    }
        //}
        //Debug.Log(preFabCheck);
        //  Debug.Log("signal er" + signal + " index er: " + ind);


        RenderTexHandler.createRenderMat(signal, ind); //create a new render material based on who joins


        // RenderTexHandler.createRenderMat(preFabCheck);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {


    }

}
