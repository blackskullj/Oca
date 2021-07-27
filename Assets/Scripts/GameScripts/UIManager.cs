using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviour
{

    #region SerializeField Variables

    [SerializeField]
    private TurnsSystemScript TurnSystem;
    [SerializeField]
    private GameObject PopUp;
    [SerializeField]
    private GameObject QuestionUI;
    [SerializeField]
    private Button[] QuestionButtons;
    [SerializeField]
    private Text[] ButtonsText;
    [SerializeField]
    private Text QuestionText;
    [SerializeField]
    private Text ResultText;
    [SerializeField]
    private Text PopUpText;
    [SerializeField]
    private Button ChangeTurnButton;
    [SerializeField]
    private PhotonView photonView;
    [SerializeField]
    private firebaseManagerScript fbManager;

    #endregion

    private GameObject Character;
    private CameraFollow camara;
    private bool PopUpActive;

    public void Start()
    {
        camara = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        UpdatePopUpText(TurnSystem.GetPlayerTurn());
        PopUpActive = true;
        ChangeTurnButton.interactable = false;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (camara.GetContTurn() == Character.GetComponent<CharacterMovement>().GetPlayerId())
            {
                if (PopUpActive)
                {
                    foreach (Player player in PhotonNetwork.PlayerList)
                    {
                        photonView.RPC("HidePopUp", player);
                    }
                }
            }
        }
    }

    [PunRPC]
    public void HidePopUp()
    {
        PopUp.SetActive(false);
        PopUpActive = false;
        ChangeTurnButton.interactable = false;
        TurnSystem.dices[0].throwButton.interactable = true;
    }

    public void OnClickChangeTurn()
    {
        Character.GetComponent<CharacterMovement>().GetComponent<Animator>().SetBool("isMoving", false);
        ShowQuestion();
    }

    private void ShowQuestion()
    {
        string question = "Hola";
        QuestionUI.SetActive(true);
        StartCoroutine(fbManager.PrepareQuestion(question));
    }

    public IEnumerator PrepareForQuestion(string question, string answer, string option1, string option2)
    {
        yield return new WaitForSeconds(1.5F);
        QuestionText.text = question;
        SetButtonsOptions(answer, option1, option2);
    }

    private void SetButtonsOptions(string answer, string option1, string option2)
    {
        System.Random random = new System.Random();
        int a, b, c;
        a = b = c = 0;
        do
        {
            a = random.Next(3);
            b = random.Next(3);
            c = random.Next(3);
        } while ((a == b) || (b == c) || (a == c));

        Debug.Log(a + " " + b + " " + c);
        Debug.Log(answer + " " + option1 + " " + option2);

        ButtonsText[a].text = answer;
        ButtonsText[b].text = option1;
        ButtonsText[c].text = option2;
        QuestionButtons[a].tag = "ButtonAnswer";
        QuestionButtons[b].tag = "ButtonOption";
        QuestionButtons[c].tag = "ButtonOption";
        /*QuestionButtons[a].GetComponent<Text>().text = answer;
        QuestionButtons[b].GetComponent<Text>().text = option1;
        QuestionButtons[c].GetComponent<Text>().text = option2;*/

    }

    /*private IEnumerator PrepareChangeTurn()
    {
        yield return new WaitForSeconds(2F);
        CallChangeTurn();
    }*/

    private void CallChangeTurn()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            photonView.RPC("ChangeTurn", player);
        }
    }

    public void OnButtonOptionClicked()
    {
        string result = EventSystem.current.currentSelectedGameObject.tag.ToString();
        if (result == "ButtonAnswer")
        {
            ResultText.text = "Correcto!";
            StartCoroutine(HideQuestionUI());
        }
        else
        {
            ResultText.text = "Incorrecto!";
            Debug.Log("Te equivocaste u.u");
            StartCoroutine(HideQuestionUI()); // Esto no lo puse al final, afuera del if-else debido a que hay castigos. si lo pongo de esa manera pueden salir problemas
        }
    }

    private IEnumerator HideQuestionUI()
    {
        yield return new WaitForSeconds(2F);
        for (int i=0; i<3; i++)
        {
            QuestionButtons[i].tag = "Untagged";
            ButtonsText[i].text = "";
        }
        QuestionText.text = "";
        ResultText.text = "";
        QuestionUI.SetActive(false);
        CallChangeTurn();
    }

    [PunRPC]
    public void ChangeTurn()
    {
        TurnSystem.UpdateTurn();
        camara.UpdatePlayerTurn(TurnSystem.GetPlayerTurn());
        UpdatePopUpText(TurnSystem.GetPlayerTurn());
        ActivatePopUp();
    }

    public void ActivatePopUp()
    {
        PopUp.SetActive(true);
        PopUpActive = true;
    }

    public void UpdatePopUpText(int PlayerTurn)
    {
        PlayerTurn++;
        PopUpText.text = "Turno del jugador " + PlayerTurn;
    }

    public void SetCharacter(GameObject Character)
    {
        this.Character = Character;
    }

}