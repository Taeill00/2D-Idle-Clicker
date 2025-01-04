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
    private static string SavePath => Application.persistentDataPath + "/Saves/"; // 영구적으로 저장할수있는 데이터 보관 루트 "Saves" 라는 폴더 생성
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
            using (StreamWriter writer = new StreamWriter(path + fileName + FileType)) // {} 가 끝나면 자동으로 메모리 해체되도록 함
            {
                BinaryFormatter formatter = new BinaryFormatter(); // 객체를 이진형식으로 변환하는 역할
                MemoryStream memoryStream = new MemoryStream(); // 임시로 직렬화된 데이터를 저장해두기 위한공간
                formatter.Serialize(memoryStream, data); // 객체를 이진형식 데이터로 변환해서 메모리스트림에써둠
                
                string dataToSave = Convert.ToBase64String(memoryStream.ToArray()); // 메모리 스트림에 쓴 이진 데이터를 문자열 형식으로 변환
                writer.Write(dataToSave); // 데이터를 실제 파일에 작성
            }
        }
    }

    public static T LoadData<T>(string fileName)
    {
        // 디렉터리가 이미 존재하면 아무작업도 수행하지 않고 빠르게 반환.성능영향 x
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
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(dataToload)); //문자열 형식에서 바이트 타입(이진형식)으로 변환해서 임시 스트림에 넣어둠

                try
                {
                    dataToReturn = (T)formatter.Deserialize(memoryStream);
                }
                catch
                {
                    // 저장되어 있는데이터가 없으면 기본생성자 형식으로 데이터를 반환
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

        GUIUtility.systemCopyBuffer = fieldExport.text; // Ctrl C 하는것
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
