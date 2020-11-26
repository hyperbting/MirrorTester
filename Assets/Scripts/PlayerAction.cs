using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAction : MonoBehaviour, INetworkSyncee
{
    [Header("Components")]
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Movement")]
    public float rotationSpeed = 100;

    public bool isLocalPlayer = false;

    [SerializeField] PlayerMainSync playerSyncData;
    public void NetworkSetup(PlayerMainSync playerMainSync)
    {
        playerSyncData = playerMainSync;

        OnJoinedRoomSync();
    }

    [SerializeField] GameObject selectedInteractable;
    // Update is called once per frame
    void Update()
    {
        // movement for local player
        if (!isLocalPlayer) return;

        // rotate
        float horizontal = Input.GetAxis("Horizontal");
        //transform.Rotate(horizontal * rotationSpeed * Time.deltaTime, 0, 0);
        Vector3 rightward = transform.TransformDirection(Vector3.right) * horizontal * Time.deltaTime;

        // move
        float vertical = Input.GetAxis("Vertical");
        //Vector3 forward = transform.TransformDirection(Vector3.forward)* Mathf.Max(vertical, 0);
        Vector3 upward = transform.TransformDirection(Vector3.up) * vertical * Time.deltaTime;

        // use NavMesh
        if (agent != null)
        {
            //agent.velocity = forward  * agent.speed;
            //animator.SetBool("Moving", agent.velocity != Vector3.zero);
        }
        else
        {
            transform.position = transform.position + upward + rightward;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            playerSyncData?.CmdTryUpdatePlayerSyncData(Time.time.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            playerSyncData?.instantiateHelper.CmdAddOne(new CreationNetworkMessage() { networkSyncType = NetworkSyncType.SceneObject});//MainService.Instance.networkCommand.CmdAddOne();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            playerSyncData?.instantiateHelper.CmdAddOne(new CreationNetworkMessage() { networkSyncType = NetworkSyncType.PlayerObject });//MainService.Instance.networkCommand.CmdAddOne();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            playerSyncData?.instantiateHelper.CmdRemoveOne(selectedInteractable);
        }

        if (Input.GetKeyDown(KeyCode.G))
            playerSyncData?.RequestAuthority(selectedInteractable);
        if (Input.GetKeyDown(KeyCode.H))
            playerSyncData?.RemoveClientAuthority(selectedInteractable);

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f) && hit.transform.tag == "Interactable")
            {
                Debug.Log("Mouse selected the " + hit.transform.name); // ensure you picked right object
                selectedInteractable = hit.transform.gameObject;
            }
            else
            {
                Debug.Log("Mouse None Selected");
                selectedInteractable = null;
            }
        }
    }

    public void OnJoinedRoomSync()
    {
        playerSyncData?.CmdTryUpdatePlayerSyncData(Time.time.ToString());
    }
}

public interface INetworkSyncee
{
    void OnJoinedRoomSync();
}