using UnityEngine;

public class UnderLetter
{
    /// <summary>
    /// �����Űܰ����Աױ�԰谳�±�������زٲ������������
    /// ���ĳʳ�봢�������ϳ׳鳻�v�ٴ��������͵ε��𵥵����
    /// ���x���Ŷǌöэ��߶춼�󶧋��󷪷����η����������ʷ��m
    /// ���ϸӸ�𹦹��¹ǹ̸޸�Őٹٹ�������̺κ��񺣺��蓎
    /// ���������ǻϻѻػڻ߻�������������żҼ�������ü��λ���
    /// �ΛX��ǽ���o�������؛y�ƾ߾����������̿����־�
    /// ����������������������������¥¹¼���ɧc���������ť�°��
    /// ��íó������������ġü��ä��īļĿ��������ťũŰ����ĳ�m
    /// Ÿ������������ƩƮƼ�����¶O����������ǥǪǻ���������л�
    /// ��������ȣȿ����������������
    /// </summary>
    static string no_underletter = "AaDdEeFfGgHhIiJjKkOoQqRrSsUuVvWwXxYyZz�����Űܰ����Աױ�԰谳�±�������زٲ�����������Ƴ��ĳʳ�봢�������ϳ׳鳻�v�ٴ��������͵ε��𵥵���۵��x���Ŷǌöэ��߶춼�󶧋��󷪷����η����������ʷ��m���ϸӸ�𹦹��¹ǹ̸޸�Őٹٹ�������̺κ��񺣺��蓎���������ǻϻѻػڻ߻�������������żҼ�������ü��λ����ΛX��ǽ���o�������؛y�ƾ߾����������̿����־�����������������������������¥¹¼���ɧc���������ť�°����íó������������ġü��ä��īļĿ��������ťũŰ����ĳ�mŸ������������ƩƮƼ�����¶O����������ǥǪǻ���������л���������ȣȿ����������������";


    public static bool HasUnderLetter(string str)
    {
        bool hasUnderletter = true;
        char c = str[^1];
        for (int i = 0; i < no_underletter.Length; i++)
        {
            if (c.Equals(no_underletter[i]))
            {
                hasUnderletter = false;
                // Debug.Log(str + "��~~  " + str + "��~~"); 
                break;
            }

        }
        if (hasUnderletter) Debug.Log(str + "��~~  " + str + "��~~");

        return hasUnderletter;
    }

    public static char SetUnderLetterEnd(string str, char endLetter)
    {
        bool hasUnderletter = HasUnderLetter(str);//��ħ�� �ִ°�? ex. �����

        switch (endLetter)
        {
            case '��':
            case '��':
                return hasUnderletter ? '��' : '��';
            case '��':
            case '��':
                return hasUnderletter ? '��' : '��';
            case '��':
            case '��':
                return hasUnderletter ? '��' : '��';
            case '��':
            case '��':
                return hasUnderletter ? '��' : '��';


        }

        return ' ';
    }

    public static string SetUnderLetter(string str, char endLetter)
    {
        return str + SetUnderLetterEnd(str, endLetter);
    }
}
