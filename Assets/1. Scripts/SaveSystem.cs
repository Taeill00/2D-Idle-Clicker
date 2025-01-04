using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    public TMP_InputField fieldImport;
    public TMP_InputField fieldExport;

    public Image imgCopyBtn;
    public Image imgPasteBtn;

    public TextMeshProUGUI textCopyBtn;
    public TextMeshProUGUI textPasteBtn;

    private const string FileType = ".txt";
    private const string FilePath = "PlayerData_Tutorial";
    private static string SavePath => Application.persistentDataPath + "/Saves/"; // ���������� �����Ҽ��ִ� ������ ���� ��Ʈ "Saves" ��� ���� ����
    private static string BackUpSavePath => Application.persistentDataPath + "/BackUps/";

    private static int SaveCount;

    public static void SaveData<T>(T data, string fileName)
    {
        Directory.CreateDirectory(SavePath);
        Directory.CreateDirectory(BackUpSavePath);

        if (SaveCount % 5 == 0)
            Save(BackUpSavePath);

        Save(SavePath);
        SaveCount++;

        void Save(string path)
        {
            using (StreamWriter writer = new StreamWriter(path + fileName + FileType)) // {} �� ������ �ڵ����� �޸� ��ü�ǵ��� ��
            {
                BinaryFormatter formatter = new BinaryFormatter(); // ��ü�� ������������ ��ȯ�ϴ� ����
                MemoryStream memoryStream = new MemoryStream(); // �ӽ÷� ����ȭ�� �����͸� �����صα� ���Ѱ���
                formatter.Serialize(memoryStream, data); // ��ü�� �������� �����ͷ� ��ȯ�ؼ� �޸𸮽�Ʈ�������
                
                string dataToSave = Convert.ToBase64String(memoryStream.ToArray()); // �޸� ��Ʈ���� �� ���� �����͸� ���ڿ� �������� ��ȯ
                writer.Write(dataToSave); // �����͸� ���� ���Ͽ� �ۼ�
            }
        }
    }

    public static T LoadData<T>(string fileName)
    {
        // ���͸��� �̹� �����ϸ� �ƹ��۾��� �������� �ʰ� ������ ��ȯ.���ɿ��� x
        Directory.CreateDirectory(SavePath);
        Directory.CreateDirectory(BackUpSavePath);

        bool backUpNeeded = false;
        T dataToReturn;

        Load(SavePath);

        if(backUpNeeded)
            Load(BackUpSavePath);

        return dataToReturn;

        void Load(string path)
        {
            using(StreamReader reader = new StreamReader(path + fileName + FileType))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                string dataToload = reader.ReadToEnd();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(dataToload)); //���ڿ� ���Ŀ��� ����Ʈ Ÿ��(��������)���� ��ȯ�ؼ� �ӽ� ��Ʈ���� �־��

                try
                {
                    dataToReturn = (T)formatter.Deserialize(memoryStream);
                }
                catch
                {
                    // ����Ǿ� �ִµ����Ͱ� ������ �⺻������ �������� �����͸� ��ȯ
                    backUpNeeded = true;
                    dataToReturn = default;
                }

                
            }
        }
    }

    public static bool SaveExists(string fileName) => File.Exists(SavePath + fileName + FileType) || File.Exists(BackUpSavePath + fileName + FileType);

    public void Import()
    {
        Directory.CreateDirectory(SavePath);

        using(StreamWriter writer = new StreamWriter(SavePath + FilePath + FileType))
        {
            writer.WriteLine(fieldImport.text);
            writer.Close();
        }

        GameController.instance.data = SaveExists(FilePath) ? LoadData<Data>(FilePath) : new Data();
    }

    public void Export()
    {
        GameController.instance.Save();
        Directory.CreateDirectory(SavePath);

        using (StreamReader reader= new StreamReader(SavePath + FilePath + FileType))
        {
            fieldExport.text = reader.ReadToEnd();
            reader.Close();
        }
    }

    public void Copy()
    {
        if (fieldExport.text == "") return;

        GUIUtility.systemCopyBuffer = fieldExport.text; // Ctrl C �ϴ°�
        imgCopyBtn.color = Color.green;
        textCopyBtn.text = "Copied!";
        StartCoroutine(CopyPasteBtnNormal());
    }

    public void Paste()
    {
        fieldImport.text = GUIUtility.systemCopyBuffer;
        imgPasteBtn.color = Color.green;
        textPasteBtn.text = "Pasted!";
        StartCoroutine(CopyPasteBtnNormal());
    }

    public void Clear(string type)
    {
        if(type == "Export")
        {
            fieldExport.text = "";
            return;
        }
        fieldImport.text = "";
    }

    private IEnumerator CopyPasteBtnNormal()
    {
        yield return new WaitForSeconds(2f);

        imgCopyBtn.color = new Color(0, 0, 0);
        textCopyBtn.text = "Copy to Clipboard";
        imgPasteBtn.color = new Color(0, 0, 0);
        textPasteBtn.text = "Paste Clipboard";
    }
}
