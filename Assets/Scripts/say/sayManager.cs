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
        sayData.Add(1, new string[] { "테스트 대사입니다.", "안녕 나는 여동생", "밥 줘 이따까리새끼야 ㅋㅋ" });
        sayData.Add(2, new string[] { "테스트 대사입니다.","뭘봐 찐따 냄새 나" });
    }

    public string GetSay(int id, int sayIndex)
    {
        if (sayIndex == sayData[id].Length)
            return null;

        else
            return sayData[id][sayIndex];
    }
}
