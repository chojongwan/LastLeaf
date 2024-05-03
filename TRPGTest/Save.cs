using Newtonsoft.Json;
using System.IO;
using TRPGTest;

internal class Save
{
    private string _fileName = "GameDataPlayer.json";

    public void SaveGameData(Player player)
    {
        string json = JsonConvert.SerializeObject(player, Formatting.Indented);
        File.WriteAllText(_fileName, json);
        Console.WriteLine("게임 데이터가 성공적으로 저장되었습니다. 아무 키나 누르면 돌아갑니다.");
        Console.ReadKey();
    }

    public Player LoadGameData()
    {
        if (File.Exists(_fileName))
        {
            string json = File.ReadAllText(_fileName);
            Player loadedPlayer = JsonConvert.DeserializeObject<Player>(json);
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
