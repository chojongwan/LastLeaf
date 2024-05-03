using System.Threading;

namespace TRPGTest
{
    public enum ItemType                // 열거형 (아이템 종류, 종류 구분으로 공격력 방어력을 구분)
    {
        Weapon,
        Armor,
        HPPotion
    }

    public class Player
    {
        public int Gold { get; set; } = 800; // 초기 골드
        public int LV { get; set; } = 1;
        public string Name { get; set; } = "";
        public string Job { get; set; } = "";
        public int Attack { get; set; } = 5;
        public int Defense { get; set; } = 5;
        public int HP { get; set; } = 100;
        public int MP { get; set; } = 50;
        public int DungeonClearCount { get; set; } = 0;     // 던전 클리어 카운트에 따른 레벨 변화
        public List<Item> Inventory { get; } = new List<Item>(); // 장비 인벤토리 리스트
        public List<Item> Backpack { get; } = new List<Item>(); // 소모품 인벤토리 리스트
    }

    public class Item          // 프로퍼티
    {
        public string Name { get; set; } // 아이템 이름
        public string Effect { get; set; } // 아이템 효과
        public string Description { get; set; } // 아이템 설명
        public int Price { get; set; } // 아이템 가격
        public ItemType Type { get; set; } // 아이템 종류
        public bool IsPurchased { get; set; } = false; // 아이템 구매 여부
        public bool IsEquipped { get; set; } // 아이템 장착 여부
        public int Amount { get; set; } = 0; // 소모품 소지 개수

        public Item(string name, string effect, string description, int price, ItemType type)
        {
            Name = name;
            Effect = effect;
            Description = description;
            Price = price;
            Type = type;
        }
    }


    public class MainMenu
    {
        // 회복 아이템 추가
        static Item[] potionItems = new Item[]
        {
        new Item("체력 30 회복 포션", "HP +30", "체력을 30만큼 회복시키는 포션입니다. (최대 HP 초과로는 회복되지 않습니다.)", 250, ItemType.HPPotion)
        };

        Status showStatus = new Status();
        Inventory inventory = new Inventory();
        Shop shop = new Shop();
        Dungeon dungeon = new Dungeon();
        Rest rest = new Rest();
        QuestManager questManager = new QuestManager();
        Player player = new Player();   // 플레이어 객체 생성

        public void CreatePlayer()  // 사용자 이름 설정
        {
            Console.Clear();
            Console.WriteLine("환영합니다! 캐릭터를 생성해주세요.");
            Console.WriteLine("캐릭터 이름을 입력해주세요.");
            Console.Write("> ");
            player.Name = Console.ReadLine();
            while (player.Name == "")
            {
                Console.Write("이름은 비워둘 수 없습니다. 다시 입력해주세요.\n> ");
                player.Name = Console.ReadLine();
            }

            chooseJob();
        }

        public void chooseJob()   // 직업 선택
        {
            string input = "";

            Console.Clear();
            Console.WriteLine($"환영합니다, {player.Name} 님!");
            Console.WriteLine("직업을 선택해주세요.\n");
            Console.WriteLine("1. 전사 | 기본 공격력 4, 방어력 6");
            Console.WriteLine("2. 궁수 | 기본 공격력 5, 방어력 5");
            Console.WriteLine("3. 마법사 | 기본 공격력 6, 방어력 4\n");

            while (input != "1" && input != "2" && input != "3")
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write("> ");

                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("전사를 선택하셨습니다.");
                        player.Job = "전사";
                        player.Attack = 4;
                        player.Defense = 6;
                        break;
                    case "2":
                        Console.WriteLine("궁수를 선택하셨습니다.");
                        player.Job = "궁수";
                        player.Attack = 5;
                        player.Defense = 5;
                        break;
                    case "3":
                        Console.WriteLine("마법사를 선택하셨습니다.");
                        player.Job = "마법사";
                        player.Attack = 6;
                        player.Defense = 4;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }

        public void ShowMainMenu()
        {
            CreatePlayer();
            
            player.Backpack.Add(potionItems[0]);
            potionItems[0].Amount = 3;  // 소모품 전용 인벤토리 player.BackPack 에 회복 아이템 3개 추가

            string input = "";

            while (input != "0")
            {
                input = "";
                Console.Clear();
                Console.WriteLine(@" $$$$$$\                                           $$$$$$\    $$\                          $$\           $$\       $$\ 
$$  __$$\                                         $$  __$$\   $$ |                         $$ |          $$ |      $$ |
$$ /  \__| $$$$$$\  $$$$$$\$$$$\   $$$$$$\        $$ /  \__|$$$$$$\    $$$$$$\   $$$$$$\ $$$$$$\         $$ |      $$ |
$$ |$$$$\  \____$$\ $$  _$$  _$$\ $$  __$$\       \$$$$$$\  \_$$  _|   \____$$\ $$  __$$\\_$$  _|        $$ |      $$ |
$$ |\_$$ | $$$$$$$ |$$ / $$ / $$ |$$$$$$$$ |       \____$$\   $$ |     $$$$$$$ |$$ |  \__| $$ |          \__|      \__|
$$ |  $$ |$$  __$$ |$$ | $$ | $$ |$$   ____|      $$\   $$ |  $$ |$$\ $$  __$$ |$$ |       $$ |$$\                     
\$$$$$$  |\$$$$$$$ |$$ | $$ | $$ |\$$$$$$$\       \$$$$$$  |  \$$$$  |\$$$$$$$ |$$ |       \$$$$  |      $$\       $$\ 
 \______/  \_______|\__| \__| \__| \_______|       \______/    \____/  \_______|\__|        \____/       \__|      \__|
                                                                                                                       
                                                                                                                       
                                                                                                                       ");                                          //시작할때 로고
                Console.WriteLine($"{player.Name} 님! 스파르타 마을에 오신 것을 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
                Console.WriteLine("0. 게임 종료");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전");
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine("6. 퀘스트\n");

                Console.WriteLine("7. 저장하기");
                Console.WriteLine("8. 불러오기\n");

                while (input != "0" && input != "1" && input != "2" && input != "3" && input != "4" &&
                    input != "5" && input != "6" && input != "7" && input != "8")
                {
                    Console.Write("원하시는 행동을 입력해주세요.\n> ");
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "0":
                            Environment.Exit(0);  // 프로그램 종료
                            break;
                        case "1":
                            showStatus.ShowStatus(player);
                            break;
                        case "2":
                            inventory.ShowInventory(player);
                            break;
                        case "3":
                            shop.ShowShop(player);
                            break;
                        case "4":
                            dungeon.ShowDungeon(player);
                            break;
                        case "5":
                            rest.GoRest(player);
                            break;
                        case "6":
                            questManager.ShowQuests();
                            break;
                        case "7":
                            Save Save = new Save();
                            Save.SaveGameData(player);
                            break;
                        case "8":
                            Save saveInstance = new Save();
                            player = saveInstance.LoadGameData();
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
            }
        }
    }
}

