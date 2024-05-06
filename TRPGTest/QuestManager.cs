using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using static TRPGTest.QuestManager;

namespace TRPGTest
{
    public class QuestManager
    {
        // 퀘스트 클래스
        public class Quest
        {
            // 프로퍼티
            public string Name { get; set; } // 퀘스트 이름
            public string Description { get; set; } // 퀘스트 설명
            public int ID { get; set; } // 퀘스트 ID
            public int Goal { get; set; } // 퀘스트 목표
            public int Progress { get; set; } // 퀘스트 진행 상황
            public bool IsComplete { get; set; } // 퀘스트 완료 여부 진행 상황(Progress)이 목표량(Goal) 
            public bool IsAccepted { get; set; } // 퀘스트 수락 여부

            // 생성자
            public Quest(string name, string description, int id, int goal)
            {
                Name = name;
                Description = description;
                ID = id;
                Goal = goal;
                Progress = 0;
                IsAccepted = false; // 처음에는 수락되지 않은 상태로 초기화
            }
        }

        private List<Quest> quests = new List<Quest>(); // 퀘스트 목록
        public Random rand = new Random();
        // 생성자
        public QuestManager()
        {
            // 퀘스트 초기화
            InitializeQuests();
        }

        // 퀘스트 초기화 메서드
        private void InitializeQuests()
        {
            // 여기에 퀘스트를 추가할 수 있습니다.
            // 예시: quests.Add(new Quest("퀘스트 이름", "퀘스트 설명", 퀘스트 번호, 목표량));
            quests.Add(new Quest("마을을 위협하는 미니언 처치", "미니언을 처치하여 마을을 지키세요.", 1, 2));
            quests.Add(new Quest("더욱 더 강해지기!", "Lv4달성.", 2, 4));
        }

        // 퀘스트 메뉴 표시
        public void ShowQuests(Player player)
        {
            Console.Clear();
            Console.WriteLine("**Quest!!**\n");

            // 수락한 퀘스트와 진행 상황 표시
            Console.WriteLine("수락한 퀘스트 목록:");
            foreach (var quest in quests)
            {
                if (quest.IsAccepted)
                {
                    Console.WriteLine($"{quest.Name}: {quest.Progress}/{quest.Goal}");
                }
            }

            Console.WriteLine("\n전체 퀘스트 목록:");
            // 퀘스트 목록 출력
            for (int i = 0; i < quests.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {quests[i].Name}");
            }

            Console.WriteLine("\n원하시는 퀘스트를 선택해주세요.");
            Console.WriteLine("-. 퀘스트 완료하기");
            Console.WriteLine("0. 뒤로가기");

            string input = Console.ReadLine();

            int questIndex;

            if (int.TryParse(input, out questIndex) && questIndex >= 1 && questIndex <= quests.Count)
            {
                // 선택한 퀘스트의 상세 정보 표시
                ShowQuestDetails(player, questIndex - 1);
            }
            else if (input == "-")
            {
                Compensation(player);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                ShowQuests(player);
            }
        }

        // 선택한 퀘스트의 상세 정보 표시
        private void ShowQuestDetails(Player player, int index)
        {
            Console.Clear();
            Console.WriteLine("**Quest!!**\n");
            Console.WriteLine(quests[index].Name + "\n");
            Console.WriteLine(quests[index].Description + "\n");
            Console.WriteLine($"- 목표량: {quests[index].Goal}\n");

            if (quests[index].IsComplete)
            {
                Console.WriteLine("퀘스트를 이미 완료하셨습니다.");
            }
            else if (quests[index].IsAccepted)
            {
                Console.WriteLine("이미 수락한 퀘스트입니다.");
            }
            else
            {
                Console.WriteLine("1. 퀘스트 수락");
            }
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    // 퀘스트 수락
                    if (!quests[index].IsAccepted)
                    {
                        AcceptQuest(index);
                        ShowQuests(player);
                    }
                    else
                    {
                        Console.WriteLine("이미 수락한 퀘스트입니다.");
                        Console.ReadKey();
                        ShowQuestDetails(player, index);
                    }
                    break;
                case "0":
                    ShowQuests(player);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    ShowQuestDetails(player, index);
                    Console.ReadKey();
                    break;
            }
        }


        // 퀘스트 수락 메서드
        private void AcceptQuest(int index)
        {
            Console.WriteLine($"퀘스트 '{quests[index].Name}'를 수락하셨습니다.");
            quests[index].IsAccepted = true;
            // 여기에 퀘스트 수락 시 추가적으로 해야 할 작업을 추가할 수 있습니다.
            Console.ReadKey();
        }

        // 몬스터가 죽을 때 호출되는 메서드
        public void MonsterDies(Player player)
        {
            foreach (var quest in quests)
            {
                if (quest.ID == 1) // 마을을 위협하는 미니언 처치 퀘스트의 ID가 1이라고 가정합니다.
                {
                    quest.Progress++;
                }
            }
        }
        public void LerverUp(Player player)
        {
            foreach (var quest in quests)
            {
                if (quest.ID == 2) // 마을을 위협하는 미니언 처치 퀘스트의 ID가 1이라고 가정합니다.
                {
                    quest.Progress++;
                }
            }
        }
        public void Compensation(Player player)
        {
            foreach (var quest in quests)
            {
                if (quest.ID == 1) // 마을을 위협하는 미니언 처치 퀘스트의 ID가 1이라고 가정합니다.
                {
                    // 퀘스트 진행 상황을 증가시킵니다.
                    if (player.DungeonClearCount >= quest.Goal) // 퀘스트의 진행 상황이 목표량에 도달하면
                    {
                        quest.IsAccepted = true; // 퀘스트를 수락한 것으로 표시합니다.
                        Console.WriteLine($"퀘스트 '{quest.Name}'를 완료하셨습니다!");
                        Console.WriteLine("보상을 받으세요!");
                        GiveQuestReward(); // 퀘스트 보상을 주는 메서드를 호출합니다.
                        Console.ReadKey();
                    }
                }
                else if (quest.ID == 2)
                {
                    if (player.LV >= 4)
                    {
                        quest.IsAccepted = true; // 퀘스트를 수락한 것으로 표시합니다.
                        Console.WriteLine($"퀘스트 '{quest.Name}'를 완료하셨습니다!");
                        Console.WriteLine("보상을 받으세요!");
                        GiveQuestReward(); // 퀘스트 보상을 주는 메서드를 호출합니다.
                        Console.ReadKey();
                    }
                }
            } 
        }
        // 퀘스트 보상 주는 메서드
        public void GiveQuestReward()
        {
            Player player = new Player();
            int gold = rand.Next(1500, 5001);
            Console.WriteLine($"골드를 {gold}G 얻었습니다!");
            player.Gold += gold;
            Console.ReadKey();
        }
    }
}

