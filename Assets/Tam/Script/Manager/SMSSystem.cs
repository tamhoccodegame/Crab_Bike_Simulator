using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SMSSystem : MonoBehaviour
{
    public static SMSSystem instance;

    public GameObject SMSUI;

    public Dictionary<string, string> smsContents;

    public GameObject SMSPrefab;
    public Transform SMSCConatainer;
    public GameObject transferSuccessfulPrefab;
    private List<GameObject> spawnedSMS = new List<GameObject>();
    public float smsDelay;
    public bool hasDebt = false;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        smsContents = new Dictionary<string, string>
        {
            { "HasPayOpening", "Hôm nay mày cần trả tao 300,000VND" },
            { "HasnotReceiveEnoughMoneyOpening", "Mày còn thiếu tao..." },
            { "HasEnoughMoneyReply", "Của mày đây" },
            { "ReceiveEnoughMoneyReply", "Tốt lắm, mai lại thế nhé" },
            { "HasnotEnoughMoneyReply", "Hôm nay tao chưa có đủ tiền" },
            { "HasnotReceiveEnoughMoney", "Được, tao cho mày tới ngày mai" },
            { "HasDonePayment", "Ok, chúng ta xong" },
            { "CannotDonePayment", "Mày tiêu đời rồi" },
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShowSMS()
    {
        SMSUI.SetActive(true);
        StartCoroutine(ShowSMS());
    }

    IEnumerator ShowSMS()
    {
        if(hasDebt)
        {

        }
        else
        if (PlayerCash.instance.CostMoney(300000))
        {
            var sms = Instantiate(SMSPrefab, SMSCConatainer);
            sms.gameObject.SetActive(true);
            sms.transform.Find("MessageBubble").Find("Background").Find("SMSText").GetComponent<TextMeshProUGUI>().text = smsContents["HasPayOpening"];
            sms.transform.Find("CreditorAvatar").gameObject.SetActive(true);
        }
        else
        {
            var sms = Instantiate(SMSPrefab, SMSCConatainer);
            sms.gameObject.SetActive(true);
            sms.transform.Find("MessageBubble").Find("Background").Find("SMSText").GetComponent<TextMeshProUGUI>().text = smsContents["HasnotReceiveEnoughMoneyOpening"];
            sms.transform.Find("CreditorAvatar").gameObject.SetActive(true);
            spawnedSMS.Add(sms);

            yield return new WaitForSeconds(1f);

            sms = Instantiate(SMSPrefab, SMSCConatainer);
            sms.gameObject.SetActive(true);
            sms.transform.Find("MessageBubble").Find("Background").Find("SMSText").GetComponent<TextMeshProUGUI>().text = smsContents["HasnotEnoughMoneyReply"];
            sms.transform.Find("PlayerAvatar").gameObject.SetActive(true);
            spawnedSMS.Add(sms);

            yield return new WaitForSeconds(3f);
            foreach(var go in spawnedSMS)
            {
                Destroy(go.gameObject);
            }
            spawnedSMS.Clear();

            GameManager.instance.SetCanChangeGameState(true);
            GameManager.instance.ChangeGameState(GameManager.GameState.Playing);
            SMSUI.SetActive(false);
        }
        yield return null;
    }

}
