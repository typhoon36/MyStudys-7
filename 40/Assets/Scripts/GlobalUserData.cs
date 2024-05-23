using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUserData 
{
    public static int g_UserGold = 0;
    public static int g_BombCount = 0;
    public static string g_NickName = "User";

    public static ulong UniqueCount = 0;    //�ӽ� Item ����Ű �߱ޱ�...
    public static List<ItemValue> g_ItemList = new List<ItemValue>();

    public static void LoadGameInfo()
    {
        g_UserGold  = PlayerPrefs.GetInt("GoldCount", 0);
        g_BombCount = PlayerPrefs.GetInt("BombCount", 0);
        g_NickName  = PlayerPrefs.GetString("UserNick", "��ɲ�");

        ReflashItemLoad();
    }

    public static ulong GetUnique() //�ӽ� ����Ű �߱ޱ�...
    {
        UniqueCount = (ulong)PlayerPrefs.GetInt("SvUnique", 0);
        UniqueCount++;
        ulong a_Index = UniqueCount;

        //<-- �ڽ��� �κ��丮�� �ִ� ������ ��ȣ�� ��ġ�� ��ȣ���ٴ� ū ����
        //UniqueID�� �߱޵ǰ� ó���ϴ� �κ�
        if(0 < g_ItemList.Count)
        {
            for(int i = 0; i < g_ItemList.Count; i++)
            {
                if (g_ItemList[i] == null)
                    continue;

                if(a_Index <= g_ItemList[i].UniqueID)
                   a_Index = g_ItemList[i].UniqueID + 1;
            }
        }//if(0 < g_ItemList.Count)

        UniqueCount = a_Index;
        PlayerPrefs.SetInt("SvUnique", (int)UniqueCount);
        return a_Index;
    }

    public static void ReflashItemLoad()  //<-- g_ItemList ����
    {
        g_ItemList.Clear();

        ItemValue a_LdNode;
        int a_ItemCount = PlayerPrefs.GetInt("Item_Count", 0);
        for(int i = 0; i < a_ItemCount; i++)
        {
            a_LdNode = new ItemValue();
            string stUniqueID = PlayerPrefs.GetString($"IT_{i}_stUniqueID", "");
            ulong.TryParse(stUniqueID, out a_LdNode.UniqueID);
            a_LdNode.m_Item_Type = (Item_Type)PlayerPrefs.GetInt($"IT_{i}_Item_Type", 0);
            a_LdNode.m_ItemName  = PlayerPrefs.GetString($"IT_{i}_ItemName", "");
            a_LdNode.m_ItemLevel = PlayerPrefs.GetInt($"IT_{i}_ItemLevel", 0);
            a_LdNode.m_ItemStar  = PlayerPrefs.GetInt($"IT_{i}_ItemStar", 0);

            g_ItemList.Add(a_LdNode);
        }
    }//public static void ReflashItemLoad()  //<-- g_ItemList ����

    public static void ReflashItemSave()  //<-- ����Ʈ �ٽ� ����
    {
        //--- ������ ����Ǿ� �־��� ������ ��� ����
        int a_ItemCount = PlayerPrefs.GetInt("Item_Count", 0);
        for(int i = 0; i < a_ItemCount + 10; i++)
        {
            PlayerPrefs.DeleteKey($"IT_{i}_stUniqueID");
            PlayerPrefs.DeleteKey($"IT_{i}_Item_Type");
            PlayerPrefs.DeleteKey($"IT_{i}_ItemName");
            PlayerPrefs.DeleteKey($"IT_{i}_ItemLevel");
            PlayerPrefs.DeleteKey($"IT_{i}_ItemStar");
        }
        PlayerPrefs.DeleteKey("Item_Count"); //������ �� ����
        PlayerPrefs.Save(); //������ ������ ������¸� Ȯ���� �����ϰ� �ϱ� ���ؼ�...
        //����� ��� Ű ���� �������� ���� ������ ����
        //PlayerPrefs�� �����͸� �޸� �� �����ϰ� �̸� �ϵ� ����̺꿡 �����Ѵ�.
        //�޸� ���� ������ �ӽ����� ��������, ���۽����� ���α׷��� ������ ���� ������ �߻����� ��,
        //������ �̷������ �ʴ� ��찡 �߻��� �� �ִ�. PlayerPrefs�� Save() �Լ��� �޸� �� �����
        //�����͸� �ϵ� ����̺꿡 �����Ѵ�.
        //--- ������ ����Ǿ� �־��� ������ ��� ����

        //--- ���ο� ����Ʈ ����
        ItemValue a_SvNode;
        PlayerPrefs.SetInt("Item_Count", g_ItemList.Count);
        for(int i = 0; i < g_ItemList.Count; i++)
        {
            a_SvNode = g_ItemList[i];
            PlayerPrefs.SetString($"IT_{i}_stUniqueID", a_SvNode.UniqueID.ToString());
            PlayerPrefs.SetInt($"IT_{i}_Item_Type", (int)a_SvNode.m_Item_Type);
            PlayerPrefs.SetString($"IT_{i}_ItemName", a_SvNode.m_ItemName);
            PlayerPrefs.SetInt($"IT_{i}_ItemLevel", a_SvNode.m_ItemLevel);
            PlayerPrefs.SetInt($"IT_{i}_ItemStar",  a_SvNode.m_ItemStar);
        }
        PlayerPrefs.Save(); //������ ������ ������¸� Ȯ���� �����ϰ� �ϱ� ���ؼ�...
        //--- ���ο� ����Ʈ ����

    }//public static void ReflashItemSave() //<-- ����Ʈ �ٽ� ����
}
