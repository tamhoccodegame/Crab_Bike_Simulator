using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SMSSystem : MonoBehaviour
{
    public static SMSSystem instance;

    public GameObject SMSUI;
    public GameObject SMSPrefab;
    public Transform SMSCConatainer;
    public GameObject transferSuccessfulPrefab;
    private List<GameObject> spawnedSMS = new List<GameObject>();
    public float smsDelay;

    public int payDay;
    public int debtDays;
    public int debtLimit;
  
    public enum SMSType
    {
        HasDebtAndTodayCanPay,
        HasDebtAndTodayCanPayAPart,
        HasDebtAndTodayCannotPay,
        TodayCanPay,
        TodayCannotPay,
        DonePayday,
        BadEnd,
    }

    public SMSType smsType;

    public Dictionary<SMSType, List<string>> smsContents = new Dictionary<SMSType, List<string>>
    {
        {
            SMSType.HasDebtAndTodayCanPay,
            new List<string>
            {
                "[C] Mày còn nợ tao {b} VND",
                "[P] Ok, tiền của mày đây",
                "[C] Tốt lắm, mai lại thế nhé!"
            }
        },

        {
            SMSType.HasDebtAndTodayCanPayAPart,
            new List<string>
            {
                "[C] Mày còn nợ tao {b} VND",
                "[P] Hôm nay tao chỉ trả trước cho mày được {a} VND thôi",
                "[C] Vậy mày còn nợ tạo {b} VND"
            }
        },

        {
            SMSType.HasDebtAndTodayCannotPay,
            new List<string>
            {
                "[C] Mày còn nợ tao {b} VND",
                "[P] Hôm nay tao vẫn chưa có đủ tiền....",
                "[C] Tao cho mày tới ngày mai..."
            }
        },

        { 
            SMSType.TodayCanPay, 
            new List<string>
            {
                "[C] Hôm nay mày cần trả tao {a} VND",
                "[P] Ok, tiền của mày đây",
                "[C] Tốt lắm, mai lại thế nhé!"
            }
        },

        { 
            SMSType.TodayCannotPay, 
            new List<string>
            {
                "[C] Hôm nay mày cần trả tao {a} VND",
                "[P] Hôm nay tao không có đủ tiền...",
                "[C] Được. Tao cho mày tới ngày mai!"
            }
        },

        {
            SMSType.DonePayday,
            new List<string>
            {
                "[C] Hôm nay mày cần trả tao {a} VND nữa là xong",
                "[P] Ok, tiền của mày đây",
                "[C] Tốt lắm! Chúng ta xong nhé!"
            }
        },

        {
            SMSType.BadEnd,
            new List<string>
            {
                "[C] Tiền của tao mày tính thế nào?",
                "[P] Tao vẫn chưa có tiền....",
                "[C] Kết thúc! Chuẩn bị gặp đàn em của tao đi!!!"
            }
        },
    };

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShowSMS()
    {
        SMSUI.SetActive(true);
        GetComponent<Image>().enabled = false;
        List<string> setenceList = new List<string>();

        setenceList = smsContents[GetSMSType()].ToList();

        StartCoroutine(ShowSMS(setenceList));
    }

    SMSType GetSMSType()
    {
        if(debtDays > 0)
        {
            if (PlayerCash.instance.CostMoney(payDay * debtDays))
                return SMSType.HasDebtAndTodayCanPay;

            if (PlayerCash.instance.CostMoney(payDay))
                return SMSType.HasDebtAndTodayCanPayAPart;

            debtDays++;
            return debtDays == 5 ? SMSType.BadEnd : SMSType.HasDebtAndTodayCannotPay;
        }

        return PlayerCash.instance.CostMoney(payDay) ? SMSType.TodayCanPay : SMSType.TodayCannotPay;
    }

    IEnumerator ShowSMS(List<string> setences)
    {
        foreach(var s in setences)
        {
            bool isPlayer = false;
            StringBuilder sb = new StringBuilder(s);

            if (s.Contains("[C]"))
            {
                sb.Replace("[C]", "");
                isPlayer = false;
            }
            else if (s.Contains("[P]"))
            {
                sb.Replace("[P]", "");
                isPlayer = true;
            }

            if (s.Contains("{a}"))
            {
                sb.Replace("{a}", payDay.ToString());
            }
            if (s.Contains("{b}"))
            {
                sb.Replace("{b}", (payDay * debtDays).ToString());
            }

            SpawnSMS(sb.ToString(), isPlayer);

            yield return new WaitForSeconds(1f);
        }


        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        foreach (var go in spawnedSMS)
        {
            Destroy(go.gameObject);
        }
        spawnedSMS.Clear();

        GameManager.instance.ChangeGameState(GameManager.GameState.Playing);
        SMSUI.SetActive(false);
        GetComponent<Image>().enabled = false;
    }

    void SpawnSMS(string text, bool isPlayer)
    {
        var sms = Instantiate(SMSPrefab, SMSCConatainer);
        sms.gameObject.SetActive(true);
        sms.transform.Find("MessageBubble").Find("Background").Find("SMSText").GetComponent<TextMeshProUGUI>().text = text;
        sms.transform.Find(isPlayer ? "PlayerAvatar" : "CreditorAvatar").gameObject.SetActive(true);
        spawnedSMS.Add(sms);
    }

}
