using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections.Generic;

/* 
Function which handles the main UI Which the master will see.
Reset when a user leaves and joins
update the latency information from the users
*/



public class RenderTextureHandler : Photon.PunBehaviour
{

    private RenderTexture rt;
    public Texture noUserConnectedTex;
    public RawImage[] img;
    private int playerIndex;
    Hashtable PlayerCustomProps = new Hashtable();
    public Text[] playerText;
    public Text masterConnectionStatus;
    float secsToNext;

    void Start()
    {
        secsToNext = 0.0f;
    }
    void Update()
    {
        //get the current connection status and display in the main GUI
        if (PhotonNetwork.isMasterClient)
        {
            masterConnectionStatus.text = "Connection status: " + PhotonNetwork.connectionStateDetailed.ToString();
        }

        //update the user latency every two seconds
        secsToNext -= Time.deltaTime;  // T.dt is secs since last update
        if (secsToNext <= 0)
        {
            updatePing();
            secsToNext = 2;
        }

    }
    
    private void updatePing()
    {
        PlayerCustomProps["Ping"] = PhotonNetwork.GetPing();

        PhotonNetwork.player.SetCustomProperties(PlayerCustomProps);
        int k = 0;

        if (PhotonNetwork.isMasterClient)
        {

            foreach (PhotonPlayer other in PhotonNetwork.playerList)
            {

                if (other.GetNext() != null && other.ID != 1)
                {

                    if (playerText[k].text != "")
                    {
                        playerText[k].text = playerText[k].text.Remove(18); //remove 18 charecters from the status text (the last ping)
                        playerText[k].text += " | " + other.customProperties["Ping"].ToString() + "ms"; //add ping from the reported from the client
                    }

                    k++;
                }
            }
        }
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        print("in player connected");

        StartCoroutine(delay(1f));

    }




    private void resetRenderTex()
    {

        int k = 2;
        foreach (RawImage renderTex in img)
        {
            //Debug.Log("in reordering");
            RenderTexture rt;
            GameObject slavePreFab = GameObject.Find("Slave_preFab" + k + "(Clone)");
            if (slavePreFab == null)
            {
                //    Debug.Log("render text er : " + renderTex);
                renderTex.texture = noUserConnectedTex;
                playerText[k - 2].text = "";
                //break;
            }
            else {
                slavePreFab.GetComponentInChildren<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("LoginGUI"));
                rt = slavePreFab.GetComponentInChildren<Camera>().targetTexture;

                playerText[k - 2].text = ("User # " + (k - 1) + " connected!");
                // Debug.Log(playerText[k - 2]);
                renderTex.texture = rt;
            }
            k++;
        }

        StopAllCoroutines();
    }
    public void resetRenderTexCall()
    {
        StartCoroutine(delay(0.2f));
    }
    private IEnumerator delay(float t)
    {
        yield return new WaitForSeconds(t);
        resetRenderTex();
    }

    public void createRenderMat(string slave, int index)
    {
        // Debug.Log(PhotonNetwork.playerList.Length);
        //Create new rendertexture  and asssign unique name
        rt = new RenderTexture(1333, 1000, 16, RenderTextureFormat.Default);
        rt.Create();
        rt.name = "renderTex " + slave; //needs to be changed 
        RenderTexture.active = rt;

        //find the slave instance which it needs to be assigned to
        // Debug.Log(slave);
        GameObject slavePreFab = GameObject.Find(slave);
        // Debug.Log(index);

        slavePreFab.GetComponentInChildren<Camera>().enabled = true;
        slavePreFab.GetComponentInChildren<Camera>().targetTexture = rt;

        //logic for assigning the rendertextures in the the main view
        if (img.Length == 0)
        {
            Debug.Log("no render textures assigned! assign them in the inspector");//if there are no textures assigned, break.
            return;
        }


        if (PhotonNetwork.playerList.Length >= 2) //apply render texture to the rawimage in the array
        {
            img[index].texture = rt;
        }



    }


}
