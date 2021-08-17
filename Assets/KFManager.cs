using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class KFManager : MonoBehaviour
{
    public static KFManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator coroutineKF;

    private int numActiveKF = 0;

    [SerializeField]
    private GameObject kfGO;
    [SerializeField]
    private Transform kfParent;

    public void StartKF(float _wait, Sprite _fixedWeapon, string _killer, string _victim, Color _c_victim, Color _c_killer, bool _HS, bool _streaking, bool _KFback)
    {
        coroutineKF = SpawnKF(_wait, _fixedWeapon, _killer, _victim, _c_victim, _c_killer, _HS, _KFback);
        Debug.Log("starting kf coroutine");
        StartCoroutine(coroutineKF);
    }

    IEnumerator SpawnKF(float _wait, Sprite _weaponType, string _killer, string _victim, Color _c_victim, Color _c_killer, bool _HS, bool _KFback)
    {
        Debug.Log("Made it to spawn kf numerator");

        if (numActiveKF == 4)
        {
            numActiveKF = 0;
        }

        Debug.Log("instantiating a thing");
        // instantiate kill feed
        GameObject kfTextGO = Instantiate(kfGO, kfParent);
        kfTextGO.name = _killer + "_kf";

        kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _killer;
        kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = _c_killer;
        kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = _weaponType;
        kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = _victim;
        kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().color = _c_victim;

        if (_HS)
        {
            kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.red;
        }

        if (!_KFback)
        {
            kfTextGO.GetComponent<Image>().enabled = false;
        }

        yield return new WaitForSeconds(_wait);
        Destroy(kfTextGO);
        numActiveKF++;
    }
}
