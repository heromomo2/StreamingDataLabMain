using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PartySharingManager : MonoBehaviour
{
    public GameObject m_shareText, m_shareButton, m_enterShareButton, m_shareInputField;

    public NetWorkClient networkClient;
    // Start is called before the first frame update
    void Start()
    {
        m_enterShareButton.GetComponent<Button>().onClick.AddListener(JoinSharingRoomButtonPressed);
        m_shareButton.GetComponent<Button>().onClick.AddListener(SendSharingRoomButtonPressed);

        //networkClient = GetComponent<NetWorkClient>();


    }

    // Update is called once per frame
    void Update()
    {

    }
    private void JoinSharingRoomButtonPressed()
    {
        string name = m_shareInputField.GetComponent<InputField>().text;
        networkClient.SendMessageToHost(ClientToServerSignifiers.JoinSharingRoom + "," + name);
        Debug.Log("JoinSharingRoomButtoPressed was called");
    }

    private void SendSharingRoomButtonPressed()
    {
        AssignmentPart2.SendOnScreenPartyToServerForSharing(networkClient);
    }
}
