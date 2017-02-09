using UnityEngine;
using System;
using System.Collections;
using UnityEngine.VR;

//script handles transfer of data from/to clients using the photonview attached to the gameobject
namespace VRStandardAssets.Utils
{
    public class DataTransferHandler : Photon.PunBehaviour
    {

        private Quaternion correctPlayerRot;

        public VRInteractiveItem interactible;
        public int sync; //variable to change the sync updates in realtime

        private Vector3 pingPos;



        [SerializeField]

        private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
                                                       // The reticle, if applicable.

        [SerializeField]
        private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField]
        private float m_DebugRayLength = 5000f;           // Debug ray length.
        [SerializeField]
        private float m_DebugRayDuration = 12f;         // How long the Debug ray will remain visible.
        [SerializeField]
        private float m_RayLength = 5000f;

        public bool isTransfering = false;
        public GameObject pingPrefab;
        private GameObject clone;

        void Start()
        {
            sync = 8;
            clone = GameObject.Find("userPing");// find the scene shared "ping" sprite. 
        }


        [PunRPC]
        private void EnablePing() //enabling the ping transfer sprite on all clients
        {

            isTransfering = true;
            //  print("in enable ping");
            SpriteRenderer sr = clone.GetComponentInChildren<SpriteRenderer>();
            Color c = sr.color;
            c.a = 170;
            sr.GetComponent<SpriteRenderer>().material.SetColor("_Color", c);
        }

        [PunRPC]
        private void DisablePing() //disable the ping transfer sprite on all clients
        {
            isTransfering = false;
            SpriteRenderer sr = clone.GetComponentInChildren<SpriteRenderer>();
            Color c = sr.color;
            c.a = 0;
            sr.GetComponent<SpriteRenderer>().material.SetColor("_Color", c);


            //    print("in disable ping");
        }



        public void userPing() //function called by whenever a client push "down" has been registeret. 
        {
            if (!isTransfering)
            {
                clone.GetComponent<PhotonView>().RequestOwnership(); //request ownership of the scene shared ping sprite. 
                photonView.RPC("EnablePing", PhotonTargets.All);
            }
            else
                photonView.RPC("DisablePing", PhotonTargets.All);
        }

        // Update is called once per frame
        public void Update()
        {
            if (PhotonNetwork.isMasterClient) //we only handle updates of other clients information on the master client
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * sync); //update the connected clients rotation (smoothing) of their headsets PoV 
                                                                                                                   //transform.position = Vector3.Lerp(transform.position, pingPos, Time.deltaTime * sync);

                if (isTransfering) //if a client is transfering ping position
                {
                    rayCast(); //calculate the ping position based on client data
                }
            }

        }

        public void rayCast()
        {

            Vector3 forward = transform.rotation * Vector3.forward; //calculate the forward vector of the client
            RaycastHit hit;
            Ray ray = new Ray(transform.position, forward); //calculate the hit on the surface

            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers))
            {

                VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object

                // If we hit an interactive item and it's not the same as the last interactive item
                if (interactible)
                {
                    // interactible.Over();

                    // Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.red, m_DebugRayDuration); //debug ray
                    Quaternion normalHit = Quaternion.FromToRotation(Vector3.forward, hit.normal); //calculate the normal of the surface to ensure a correct rotation of the ping sprite 
                    photonView.RPC("transferPing", PhotonTargets.All, hit.point, normalHit); //transfer the hit data to all clients


                    // m_ReticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                }

            }


        }
        [PunRPC]
        public void transferPing(Vector3 pos, Quaternion forwardN)
        {
            // receive information of the ping position from master client
            pos.Scale(new Vector3(0.99f, 0.99f, 0.99f)); //scale the vector so its inside the sphere (biased slightly so we dont see any overllaping geomtry issues)
                                                         //update the postion on the shared ping sprite
            clone.transform.position = pos;
            clone.transform.rotation = forwardN;

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.isWriting) //this client is sending this to all clients
            {

                //stream.SendNext(InputTracking.GetLocalRotation(VRNode.Head)); //Debug.Log("Serialize is sending something");
                stream.SendNext(transform.rotation);//send the transform data (rotation). 
                stream.SendNext(transform.position);// send camera position

            }

            else {

               correctPlayerRot = (Quaternion)stream.ReceiveNext(); //if im not sending, i am receivning data
                pingPos = (Vector3)stream.ReceiveNext();

            }

        }
    }
}

