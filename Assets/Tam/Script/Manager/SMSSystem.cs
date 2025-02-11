using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SMSSystem : MonoBehaviour
{
    public static SMSSystem instance;

    public Dictionary<string, string> smsContents;

    public GameObject SMSPrefab;
    public Transform SMSCConatainer;
    public GameObject transferSuccessfulPrefab;

    public float smsDelay;
    public bool hasDebt = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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

        StartCoroutine(ShowSMS());
    }

    // Update is called once per frame
    void Update()
    {
        
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

            yield return new WaitForSeconds(1f);

            sms = Instantiate(SMSPrefab, SMSCConatainer);
            sms.gameObject.SetActive(true);
            sms.transform.Find("MessageBubble").Find("Background").Find("SMSText").GetComponent<TextMeshProUGUI>().text = smsContents["HasnotEnoughMoneyReply"];
            sms.transform.Find("PlayerAvatar").gameObject.SetActive(true);
        }
        yield return null;
    }

}
