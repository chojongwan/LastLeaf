using Newtonsoft.Json;
using System.IO;
using TRPGTest;
using static TRPGTest.QuestManager;

internal class Save
{
    private string _fileName = "Player.json";
    //private string _fileName1 = "Quest.json";

    public void SaveGameData(Player player) //, Quest quest
    {
        string json = JsonConvert.SerializeObject(player, Formatting.Indented);
        File.WriteAllText(_fileName, json);
        //string json1 = JsonConvert.SerializeObject(quest, Formatting.Indented);
        //File.WriteAllText(_fileName1, json1);
        Console.WriteLine("게임 데이터가 성공적으로 저장되었습니다. 아무 키나 누르면 돌아갑니다.");
        Console.ReadKey();
    }

    public Player LoadGameData()
    {
        if (File.Exists(_fileName)) //&& File.Exists(_fileName1)
        {
            string json = File.ReadAllText(_fileName);
            Player loadedPlayer = JsonConvert.DeserializeObject<Player>(json);
            //string json1 = File.ReadAllText(_fileName1);
            //Quest loadedQuest = JsonConvert.DeserializeObject<Quest>(json1);
            Console.WriteLine("게임 데이터가 성공적으로 불러와졌습니다. 아무 키나 누르면 돌아갑니다.");
            Console.ReadKey();
            return loadedPlayer;
        }
        else
        {
            Console.WriteLine("저장된 게임 데이터가 없습니다. 아무 키나 누르면 돌아갑니다.");
            Console.ReadKey();
            return null;
        }
    }

}
