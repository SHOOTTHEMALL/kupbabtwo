using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sayManager : MonoBehaviour
{
    Dictionary<int, string[]> sayData;
    // Start is called before the first frame update
    void Awake()
    {
        sayData = new Dictionary<int, string[]>();
        GeData();
    }

    // Update is called once per frame
    void GeData()
    {
        sayData.Add(1, new string[] { "�׽�Ʈ ����Դϴ�.", "�ȳ� ���� ������", "�� �� �̵�������� ����" });
        sayData.Add(2, new string[] { "�׽�Ʈ ����Դϴ�.","���� ��� ���� ��" });
    }

    public string GetSay(int id, int sayIndex)
    {
        if (sayIndex == sayData[id].Length)
            return null;

        else
            return sayData[id][sayIndex];
    }
}
