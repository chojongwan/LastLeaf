using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TRPGTest
{
    internal class Inventory
    {
        string input = "";


        // 인벤토리 표시 메서드
        public void ShowInventory(Player player)
        {
            input = "";
            while (input != "0")
            {
                Console.Clear();
                Console.WriteLine("장비 인벤토리");
                Console.WriteLine("보유 중인 장비를 관리할 수 있습니다.\n");

                if (player.Inventory.Count == 0)  // Inventory 아이템 리스트가 비어있을 때
                {
                    Console.WriteLine("보유 중인 장비가 없습니다.");
                    Console.WriteLine("-. 소모품 가방");
                    Console.WriteLine("0. 나가기\n");

                    while (input != "0")
                    {
                        Console.Write("원하시는 행동을 입력해 주세요.\n>> ");
                        input = Console.ReadLine();

                        if (input == "-" || input == "_")
                        {
                            ShowBackpack(player);  // 소모품 인벤토리 호출
                            ShowInventory(player);
                        }
                        else if (input != "0")
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }
                else  // Inventory 에 아이템이 있을 때
                {
                    Console.WriteLine("[장비 목록]");
                    for (int i = 0; i < player.Inventory.Count; i++)
                    {
                        string equipped = player.Inventory[i].IsEquipped ? "[E]" : "";                  // 아이템이 장착되었는지 확인하여 표시, 삼항 연산자-> equipped
                        Console.WriteLine($"{i + 1}. {equipped}{player.Inventory[i].Name}");
                    }
                    Console.WriteLine();
                    Console.WriteLine("장착 상태를 변경할 장비의 번호를 입력해주세요.");
                    Console.WriteLine("-. 소모품 가방\n");
                    Console.WriteLine("0. 나가기\n");

                    while (input != "0")
                    {
                        Console.Write("원하시는 행동을 입력해 주세요.\n>> ");
                        input = Console.ReadLine();

                        if (input == "-" || input == "_")
                        {
                            ShowBackpack(player);  // 소모품 인벤토리 호출
                            ShowInventory(player);
                        }
                        else if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= player.Inventory.Count)        //selectedIndex 인벤토리에서 해당 아이템을 찾을때 사용 (out 인자를 사용)
                        {
                            selectedIndex -= 1;  // -1 을 해서 배열 정렬

                            Item selected = player.Inventory[selectedIndex];
                            if (selected.IsEquipped)
                            {
                                UnequipItem(selected, player);  // 이미 장착된 아이템일 시 장착 해제
                                Console.WriteLine($"{selected.Name}을(를) 해제했습니다. 아무 키나 누르시면 새로고침됩니다.");
                                Console.ReadKey();
                                ShowInventory(player);
                            }
                            else
                            {
                                EquipItem(selected, player);  // 선택한 아이템 장착
                                Console.WriteLine($"{selected.Name}을(를) 장착했습니다. 아무 키나 누르시면 새로고침됩니다.");
                                Console.ReadKey();
                                ShowInventory(player);
                            }
                        }
                        else if (input != "0")
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }
            }
        }


        // 아이템 장착 메서드
        public void EquipItem(Item selected, Player player)
        {
            if (selected.IsEquipped)  // 다른 코드에서 중복 장착 방지용, 이미 장착된 아이템일 시 장착 해제
            {
                UnequipItem(selected, player);
            }
            else
            {
                // 이미 장착된 아이템의 수를 셈
                int equippedItemCount = 0;
                foreach (var item in player.Inventory)
                {
                    if (item.IsEquipped)
                        equippedItemCount++;
                }

                // 이미 2개의 아이템을 장착했는지 확인
                if (equippedItemCount >= 2)
                {
                    Console.WriteLine("더 이상 장비를 장착할 수 없습니다.");
                }
                else
                {
                    // 선택한 아이템 장착
                    selected.IsEquipped = true;

                    // 선택한 아이템의 효과를 능력치에 반영
                    switch (selected.Type)
                    {
                        case ItemType.Weapon:
                            player.Attack += GetEffectValue(selected.Effect);
                            break;
                        case ItemType.Armor:
                            player.Defense += GetEffectValue(selected.Effect);
                            break;
                    }
                }
            }
        }


        // 아이템 장착 해제 메서드
        public void UnequipItem(Item selected, Player player)
        {
            // 선택한 아이템 해제
            selected.IsEquipped = false;
            // 선택한 아이템의 효과를 능력치에서 제거
            switch (selected.Type)
            {
                case ItemType.Weapon:
                    player.Attack -= GetEffectValue(selected.Effect);
                    break;
                case ItemType.Armor:
                    player.Defense -= GetEffectValue(selected.Effect);
                    break;
            }
        }


        // 소모품 인벤토리
        public void ShowBackpack(Player player)
        {
            input = "";
            while (input != "0")
            {
                Console.Clear();
                Console.WriteLine("소모품 가방");
                Console.WriteLine("보유 중인 소모품을 사용할 수 있습니다.\n");

                if (player.Backpack.Count == 0)  // Backpack 아이템 리스트가 비어있을 때
                {
                    Console.WriteLine("보유 중인 소모품이 없습니다.");
                    Console.WriteLine("0. 나가기\n");

                    while (input != "0")
                    {
                        Console.Write("원하시는 행동을 입력해 주세요.\n>> ");
                        input = Console.ReadLine();

                        if (input != "0")
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }
                else  // Backpack 에 아이템이 있을 때
                {
                    Console.WriteLine("[소모품 목록]");
                    for (int i = 0; i < player.Backpack.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {player.Backpack[i].Name} | 보유 {player.Backpack[i].Amount}개");
                    }
                    Console.WriteLine();
                    Console.WriteLine("사용할 소모품의 번호를 입력해주세요.");
                    Console.WriteLine("0. 나가기\n");

                    while (input != "0")
                    {
                        Console.Write("원하시는 행동을 입력해 주세요.\n>> ");
                        input = Console.ReadLine();

                        if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= player.Backpack.Count)        //selectedIndex 인벤토리에서 해당 아이템을 찾을때 사용 (out 인자를 사용)
                        {
                            selectedIndex -= 1;  // -1 을 해서 배열 정렬

                            Item selected = player.Backpack[selectedIndex];

                            UseItem(selected, player);  // 아이템 사용
                            Console.WriteLine($"{selected.Name}을(를) 사용했습니다. 아무 키나 누르시면 새로고침됩니다.");
                            Console.ReadKey();
                            ShowBackpack(player);
                        }
                        else if (input != "0")
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }
            }
        }


        // 소모품 사용
        public void UseItem(Item selected, Player player)
        {
            switch (selected.Type)
            {
                case ItemType.HPPotion:
                    if (player.HP + GetEffectValue(selected.Effect) > 100)  // 회복 이후 체력이 최대 체력보다 높다면
                    {
                        Console.WriteLine($"체력: {player.HP} -> 100");  // 최대 체력으로 설정
                        player.HP = 100;
                    }
                    else
                    {
                        Console.WriteLine($"체력: {player.HP} -> {player.HP + GetEffectValue(selected.Effect)}");
                        player.HP += GetEffectValue(selected.Effect);
                    }
                    break;
            }

            selected.Amount -= 1;
            if (selected.Amount <= 0)
            {
                player.Backpack.Remove(selected);
            }
        }


        // 아이템 효과 값을 가져오는 메서드
        public int GetEffectValue(string effect)
        {
            int value = 0;
            foreach (char c in effect)
            {
                if (char.IsDigit(c))
                {
                    value = value * 10 + (c - '0');
                }
            }
            return value;
        }
    }
}
