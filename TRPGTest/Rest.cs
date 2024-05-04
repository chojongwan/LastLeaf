using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace TRPGTest
{
    internal class Rest
    {
        public void GoRest(Player player)
        {
            Console.Clear();
            string input = "";

            while (input != "0")
            {
                Console.WriteLine("휴식하기");
                Console.WriteLine($"500 G 를 내면 체력과 마력을 회복할 수 있습니다.\n");
                Console.WriteLine($"현재 체력: {player.HP}\n현재 마력: {player.MP}\n보유 골드: {player.Gold} G\n");

                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기\n");

                while (input != "0")
                {
                    Console.Write("원하시는 행동을 입력해주세요.\n> ");
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            if (player.Gold < 500)
                            {
                                Console.WriteLine("Gold 가 부족합니다.");
                            }
                            else if (player.HP >= 100 && player.MP >= 50)
                            {
                                Console.WriteLine("이미 체력과 마력이 최대입니다.");
                            }
                            else
                            {
                                player.Gold -= 500; // 골드 차감
                                player.HP = Math.Min(player.HP + 100, 100); // 최대 체력 100으로 제한하여 회복
                                player.MP = Math.Min(player.MP + 50, 50); // 최대 마력 50으로 제한하여 회복
                                Console.WriteLine($"휴식을 완료했습니다. (현재 체력: {player.HP}, 현재 마력: {player.MP}, 보유 골드: {player.Gold} G)");
                            }
                            break;
                        case "0":
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
