using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPGTest
{
    internal class Status
    {
        // 상태 보기 기능 구현
        public void ShowStatus(Player player)
        {
            string input = "";
            while (input != "0")
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                Console.WriteLine($"Lv: {player.LV.ToString("D2")}");
                Console.WriteLine($"이름: {player.Name} (직업: {player.Job})");
                Console.WriteLine($"공격력: {player.Attack}");
                Console.WriteLine($"방어력: {player.Defense}");
                Console.WriteLine($"체력: {player.HP}");
                Console.WriteLine($"마력: {player.MP}");
                Console.WriteLine($"Gold: {player.Gold} G\n");
                Console.WriteLine("-. 관리자 모드");
                Console.WriteLine("0. 나가기\n");

                while (input != "0")
                {
                    Console.Write("원하시는 행동을 입력해주세요.\n> ");
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "0":
                            break;
                        case "-":
                            string code = "";
                            while (code != "0" && code != "allmaster")
                            {
                                Console.WriteLine("관리자 코드를 입력해주세요.");
                                Console.WriteLine("0. 나가기");
                                Console.Write("> ");
                                code = Console.ReadLine();

                                if (code == "allmaster")
                                {
                                    player.Gold = 100004;
                                    player.Attack = 1004;
                                    player.Defense = 1004;
                                    Console.WriteLine("관리자 모드가 설정되었습니다.");
                                }
                                else if (code != "0")
                                {
                                    Console.WriteLine("코드가 다릅니다.");
                                }
                            }
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
