using System;
using System.Collections;
using System.Collections.Generic;

namespace TRPGTest
{
    internal class Dungeon
    {
        public class Monster
        {
            public string MonsterName { get; set; } // 몬스터 이름
            public int MonsterLV { get; set; } // 몬스터 레벨
            public int MonsterHP { get; set; } // 몬스터 체력
            public int AttackMonster { get; set; } // 몬스터 공격력
            public bool MonsterDie { get; set; } // 몬스터 생사구분
            public int MonsterCount { get; set; }
        }
        public int monsterDieCount = 0;
        public int stage = 0;
        public int monsterCount = 0;
        List<Monster> monsters; // 몬스터 리스트
        public Random rand = new Random();
        QuestManager quest;
        // 랜덤 몬스터 생성 메서드
        public Monster CreateRandomMonster()
        {
            Random rand = new Random();
            int level = rand.Next(2, 6); // 랜덤한 레벨 (2~5)
            string name = "";
            int hp = 0;
            int atk = 0;
            if (stage == 5)
            {
                level = 6;
            }
            else if (stage == 10)
            {
                level = 7;
            }
            else if (stage == 15)
            {
                level = 8;
            }
            switch (level)
            {
                case 2:
                    name = "세이렌";
                    hp = rand.Next(10, 21); // 10~20 사이의 랜덤한 체력
                    atk = rand.Next(3, 7); // 3~6 사이의 랜덤한 공격력
                    break;
                case 3:
                    name = "켄타우로스";
                    hp = rand.Next(8, 16); // 8~15 사이의 랜덤한 체력
                    atk = rand.Next(6, 10); // 6~9 사이의 랜덤한 공격력
                    break;
                case 4:
                    name = "키클롭스";
                    hp = rand.Next(20, 31); // 20~30 사이의 랜덤한 체력
                    atk = rand.Next(7, 12); // 7~11 사이의 랜덤한 공격력
                    break;
                case 5:
                    name = "기가스";
                    hp = rand.Next(25, 36); // 25~35 사이의 랜덤한 체력
                    atk = rand.Next(10, 16); // 10~15 사이의 랜덤한 공격력
                    break;
                case 6:
                    name = "하데스";
                    hp = rand.Next(100, 200);
                    atk = rand.Next(50, 100);
                    break;
                case 7:
                    name = "포세이돈";
                    hp = rand.Next(200, 400);
                    atk = rand.Next(100, 200);
                    break;
                case 8:
                    name = "제우스";
                    hp = rand.Next(300, 500);
                    atk = rand.Next(150, 300);
                    break;
            }

            Monster newMonster = new Monster();
            newMonster.MonsterName = name;
            newMonster.MonsterLV = level;
            newMonster.MonsterHP = hp;
            newMonster.AttackMonster = atk;

            return newMonster;

        }
        public Dungeon(QuestManager questManager)
        {
            monsters = new List<Monster>(); // 몬스터 리스트 초기화
            quest=questManager;
        }

        public void ShowDungeon(Player player)
        {
            string input = "";
            while (input != "0")
            {
                input = "";

                Console.Clear();
                if (player.HP <= 0) // HP가 0이하인 경우 게임 오버
                {
                    GameOver(player);
                    break;
                }
                Console.WriteLine("던전");
                Console.WriteLine("전투로 골드를 얻을 수 있는 던전입니다.\n");
                Console.WriteLine($"[현재 HP] {player.HP} \n");
                Console.WriteLine($"[현재 Gold] {player.Gold}G \n");
                Console.WriteLine("1. 전투 시작");
                Console.WriteLine("0. 나가기");
                input = Console.ReadLine();
                if (input == "1")
                    StartBattle(player); // 전투 시작
                else
                    Console.WriteLine("잘못된 입력입니다.");

            }
        }

        // 전투 시작
        public void StartBattle(Player player)
        {
            Console.Clear();
            Console.WriteLine("**Battle!!**");

            // 몬스터가 생성되지 않은 경우에만 몬스터 생성
            if (monsters.Count == 0)
            {
                stage++;
                if (stage == 5)
                {
                    Console.WriteLine(@"

                     ,~~-.. ,..,~~-.              
                 -:-,    ..,       .,,:*$***:,.   
            ...,,~~~,,----~---~~---~*==!****$$!~  
          ..-~~~:**!!!*=======$#=!=$==#*=;~=*$:*- 
        ..-~:!*!;:::!***=====*==**=$*$@*$;~=!=;=~,
       -~,.,,--,....,-----:===*=====*==!;~*$===*,.
      ...                  ,,,!==*==!;===*=#@=*-  
                              ...:=**!$#$#=:*$~   
                           !*$-    ,!===*#=:!=-   
                          :===$*.     ==$@#$*=~   
                         ,!!***#=~.   ,===#$#*,   
                         ~*-,~:=;$;   ~;:$!::-    
                        ~**~-:==*!:   ~!,*        
                       :*=!~::*!*:.   :;:-        
                     -;=$$=:!;;;=*.  .;~$         
         ,          -*$#$#$*;;:;#$-  ~;!,         
         =:     -:;;.=##$##*;:~;@#:  ;:$          
         #@*.,~-  ,-;@@##@@$:!!$@@;.-;*           
         #=#;~,,,-=:-=##=$#@@;;#@@$:-;;           
         =$!*;~-,:;:!-*#$$$##=$=@#$=:!.           
          ~!!;;:;=:-~-:*$####:*!=##!:.            
            ,!!!;!!~::*!=#@##*!;!$#*-             
            ,*$===*!;**;!$$*$*!*$$$=;.            
             :####=*$#$=!*=====*==$$!-            
            ,=#==##=$==!=$!!==!*!=#=*-            
          @$;@@@@@$==*!~~!=***#@@#$==-            
          ,#@@##@@=$!==*:;;*$,===$==$$.           
           ,$$$$$=:*=$$$$=*$======$$=*-           
               .,~:==$$#$=!**=====$$=$#           
              .~*!*=$$##$==*======$$$#$,        . 
              -=#===$$#$$$=$$=$$$$$$$$*;       ~;~
              :$#$==!~*$$#$$$$#######$=$     ,~==;
             ~*#$==:. :!###$$##$**###$#=;:~,~;*~,.
           .!!$#$==,~-=$##$$##!*::*##@@===**=;,   
           *$=$#=$@@:*;;$##===!*;:;*-==*==#!      
           **=$$=$#@#  ,!*$=*$*****=*:=!##$~      
          .:@$$$=$##@=,  ,*=$=$===$$$*=$$#=       
        .~!!##$$==@#@@$~.,=$==$$$##$$=*=$$*       
        ~=*:*$=#$$@@#@@@*!$#=##$##$=$$**@@=       
       ,:==:$$=#$$@@#@@@@###$#$$#$=$$==*=@$.      
       -!;!*@==###@@@@@@@==$$$=$=$$=$$$$$@#-      
       -$**=$*=###@@@@@@@#$$$====$$=$$$$$$#~      
        -:==*-$####@@@@@@###$===$$=$##$$$$#!      
       .~=.$=:$####@@@@@#$####$=$$$$###$$$$=,     
      .~*   .!$###$@@@@@#$#######$$#########*.     
");
                    Console.WriteLine("황천의 지배자 하데스(Ἅιδης)가 나타났습니다!");
                    monsterCount++;
                    monsters.Add(CreateRandomMonster());
                    monsterDieCount++;
                }
                else if (stage == 10)
                {
                    Console.WriteLine(@"                                         
       .                                          
       *      ,                                   
       ;-    ..                                   
       ::    *                                    
 -     !:.  ,:                                    
 .=.   !.~  :!                                    
  ,!.  !.:  ;*                                    
   !:. :,~ ,,=              ;                     
   .~* --.~~ *              ,,                    
    !,:.~.~~ *            :  .,!                  
     -~.:.:~ *            .~ - :                  
     *,!~.-; =           -!=,!*.                  
     .-~:,,,,*          ! .,;:,#                  
      ;,~:-:,,         ~ *!!.~;#!                 
      ;-:!~~          ,, @,    *#                 
         ;,;          ;,,@,,,,-~@                 
         --~          = @@-:~;;;@                 
         .;-         .,..@-,-~.;@.                
          !-          ;;=@;~.,~#@.  .....         
          ~,,       ,!$. @ -:::@@.     ,--        
          :,!        ~.=:$  ..!@@,      .--.      
          --:       .;:!.:    :@@#       .~-.     
          .!-:    ,; .~~=-  : !@@@#       ~-~     
         .-.,*;   !.  ,$~#. ;!@@@@@$=   - ,--~    
         - .-::   ;~.   ~*;!=;::~~,  :~ .-.-~-    
         -..-:,   :;.    ~*-          !. -----.   
          ;..:!. :!!     ~,        .  .~ .----,   
          ~.:*@@@$=!     ;.        *-,,: .----~   
           *=,@@@@@!.   ,$     . .,!;~-; .----~   
            *,@@@@@*--,--#-..   ,!:,,~ .;.-----   
            --@@@@@@:-::!$;~-,.,~@-. ;- -~----,   
            .-;$@@@#@# ... ,!::!@@:. .$: !----,   
             !, .$! ::,-~;-. *##@@@~..~* ;----,   
             =      ~-*:  .*=,!*@@*=#!. .-~---.   
             ~-- ----=, ,,  ,~*$@@~=  *  .!--..   
             ,-$-----!-,~$:-; ~$@- .-@@* ,=--.  . 
            .-~;--,..: =-;.-, ,!@:: @@@@-~*~..    
           ,~~*.-... :,=,$ .; ,;@# #@@@@*;-,.. .  
          .--.= -..  =-..:, .-;~-.:@@@@#:-,.. ..  
         .--.-;-~.  .*-,   ,:,.,-  #@$!~-,......                             
");
                    Console.WriteLine("바다의 지배자 포세이돈(Ποσειδῶν)이 나타났습니다!");
                    monsterCount++;
                    monsters.Add(CreateRandomMonster());
                    monsterDieCount++;
                }
                else if (stage == 15)
                {
                    Console.WriteLine(@"

..                                                
 ,:                                               
  .=-  ~                                          
    *$  @-                                        
     :@;-@=                                       
      .@@@@@.;                                    
        @@#*@=@.,@;                               
         $@-,@@!@!$#.                             
          ;#  =*@@@@@ .;                          
           -:  ~@@@:;, @=                         
               ,@@@!@=.#@#.                       
                =@@;#@@#@@@,                      
               .@*** ;@@@=@@:                     
                =$*$. .$@ ~@@!                    
                -@@@:   -.  :@=                   
                 @@@!,       .=#.          :*$,   
                -@@@@.         ,#-       $ ,!@$*= 
                 ,!-*@           ::     #=  .~ ;@#
                 :@@@@-            ~    -@#@@@@#;$
                 .@@@@@~..             $#@@$$@@@# 
                  $@@@@=@@@@-,--.      #$!!$@~$@; 
                  -@@@@*@@@@@!#@@*   --@*@#$@#=;  
                   =@@@@@@@@@@!@@@=.:-;@:=**@@@#  
                    @@@@@@@@@@##@@@;*$=;@#-@@@#$  
                    -=@@@@@@@@@@@@@=@$@@#: @$*:   
                       ,$@@@@@@@$@@*:;#@#= !@$.   
                         ~@@@@@@*@#-:!!@@$ $@-    
                            .:;@*@*~*$-#@@-#@*    
                             ,$$!@!:@:@;*@@@@@-   
                              =#$;:*@:$@@,@@@@    
");
                    Console.WriteLine("하늘의 지배자 제우스(Ζεύς)가 나타났습니다!");
                    monsterCount++;
                    monsters.Add(CreateRandomMonster());
                    monsterDieCount++;
                }
                else
                {
                    // 몬스터 랜덤 등장 (1~4마리)
                    monsterCount = rand.Next(1, 5); // 1~4 사이의 랜덤한 몬스터 수
                    Console.WriteLine("총 {0}마리의 몬스터가 등장했습니다.\n", monsterCount);
                    // 랜덤 몬스터 생성
                    for (int i = 0; i < monsterCount; i++)
                    {
                        monsters.Add(CreateRandomMonster());
                        monsterDieCount++;
                    }
                }
            }

            // 플레이어 정보 출력
            Console.WriteLine("[내정보]");
            Console.WriteLine("Lv.{0} {1} ({2})", player.LV, player.Name, player.Job);
            Console.WriteLine("HP: {0}/{1}\n", player.HP, 100);
            Console.WriteLine("MP: {0}/{1}\n", player.MP, 50);
            Console.WriteLine($"[현재 Gold] {player.Gold}G \n");
            Console.WriteLine($"현재 스테이지 {stage}층 \n");



            // 몬스터 정보 출력
            Console.WriteLine("[몬스터]");

            for (int i = 0; i < monsterCount; i++)
            {
                Console.WriteLine("{0}. Lv.{1} {2}  HP {3}", i + 1, monsters[i].MonsterLV, monsters[i].MonsterName, monsters[i].MonsterHP);
            }

            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 스킬");

            Console.WriteLine("3. 다음 전투하기");
            Console.WriteLine("0. 도망치기");
            string input = Console.ReadLine();

            // 공격 선택
            if (input == "1")
            {
                AttackMonster(player, monsters);
            }
            else if (input == "2")
            {
                Skill(player, monsters);
            }
            else if (input == "3")
            {
                if (monsterDieCount == 0)
                {
                    EnemyPhase(player, monsters);
                }
                else
                {
                    Console.WriteLine("모든 몬스터를 처리하지 않았습니다!");
                    Console.Read();
                    StartBattle(player);
                }
            }
            else if (input == "0")
            {
                // 다음 페이즈 진행
                EnemyPhase(player, monsters);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                StartBattle(player);
            }
        }

        // 몬스터 공격 메서드
        public void AttackMonster(Player player, List<Monster> monsters)
        {
            // 몬스터 선택
            Console.WriteLine("\n대상을 선택해주세요.");
            int targetIndex;
            if (!int.TryParse(Console.ReadLine(), out targetIndex) || targetIndex < 1 || targetIndex > monsters.Count)
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
                StartBattle(player);
            }

            // 선택한 몬스터 인덱스 계산 (0부터 시작하는 인덱스로 변환)
            int monsterIndex = targetIndex - 1;

            // 선택한 몬스터가 이미 죽은 경우
            if (monsters[monsterIndex].MonsterDie)
            {
                Console.WriteLine("이미 죽은 몬스터를 공격할 수 없습니다.");
                Console.ReadKey();
                StartBattle(player);
            }

            // 몬스터 공격 if문으로 치명타를 설정가능
            int criticalprobability = rand.Next(1, 11);//치명타 확률
            if (criticalprobability > 7)
            {
                Console.WriteLine("치명타 발동!");
                int criticaldamage = CriticalHit(player.Attack);
                monsters[monsterIndex].MonsterHP -= criticaldamage;
                Console.WriteLine("\nChad 의 공격!");// 공격 결과 출력
                Console.WriteLine("{0} 을(를) 맞췄습니다. [데미지 : {1}]", monsters[monsterIndex].MonsterName, criticaldamage);
                Console.ReadKey();
            }
            else
            {
                int damage = CalculateDamage(player.Attack);
                monsters[monsterIndex].MonsterHP -= damage;
                Console.WriteLine("\nChad 의 공격!");// 공격 결과 출력
                Console.WriteLine("{0} 을(를) 맞췄습니다. [데미지 : {1}]", monsters[monsterIndex].MonsterName, damage);
                Console.ReadKey();
            }
            // 몬스터가 죽은 경우
            if (monsters[monsterIndex].MonsterHP <= 0)
            {
                monsters[monsterIndex].MonsterHP = 0; // HP가 0 아래로 내려가지 않도록 보정
                monsters[monsterIndex].MonsterDie = true;

                Console.WriteLine("{0}이(가) 사망했습니다.", monsters[monsterIndex].MonsterName);
                monsterDieCount--;
                player.DungeonClearCount++;
                if ((player.DungeonClearCount / 5) >= player.LV)  //레벨 오르는 부분
                {
                    player.LV++;
                    player.Attack += 1; // 공격력 증가
                    player.Defense += 2; // 방어력 증가
                    Console.WriteLine($"레벨이 올랐습니다. 현재 레벨: {player.LV}, 공격력: {player.Attack}, 방어력: {player.Defense}");
                }
                Console.WriteLine("몬스터를 처치하여 소량의 골드를 얻었습니다.");
                int gold = rand.Next(100, 1001);
                Console.WriteLine($"골드를 {gold}G 얻었습니다!");
                player.Gold += gold;
                quest.MonsterDies(player);
                Console.ReadKey();

                StartBattle(player);
                // 모든 몬스터가 죽었는지 확인
                bool allMonstersDead = true;
                foreach (var monster in monsters)
                {
                    if (!monster.MonsterDie)
                    {
                        monster.MonsterCount = 0;
                        allMonstersDead = false;
                        break;
                    }
                }
                // 모든 몬스터가 죽은 경우 다음 랜덤 스폰
                if (allMonstersDead)
                {
                    EnemyPhase(player, monsters);
                    monsterCount = 0;
                    monsters.Clear();
                }
            }
            else
            {
                // 전투 다음 페이즈 진행
                EnemyPhase(player, monsters);
            }
        }
        // 공격력 계산 메서드 (10% 오차 적용)
        public int CalculateDamage(int attack)
        {
            Random rand = new Random();
            double error = rand.NextDouble() * 0.2 - 0.1; // -0.1부터 0.1까지의 오차
            int finalDamage = (int)Math.Ceiling(attack * (1 + error)); // 최종 공격력 계산 (오차 적용)
            return finalDamage;
        }
        //치명타 공격 메서드
        public int CriticalHit(int attack)
        {
            int critical = new Random().Next(1, 100);
            int criticaldamage = 0;
            bool isCritical = false;
            if (critical >= 15)
            {
                isCritical = true;
                double newCriticalattack = attack * 1.6;
                criticaldamage = (int)Math.Round(newCriticalattack);
            }
            else
            {
                isCritical = false;
            }
            int itcriticaldamage = (int)criticaldamage;
            return itcriticaldamage;
        }
        // 스킬 공격 메서드
        public void Skill(Player player, List<Monster> monsters)
        {
            Console.WriteLine(" ");
            Console.WriteLine("1. 알파 스트라이크 - MP 10");
            Console.WriteLine("공격력 * 2 로 하나의 적을 공격합니다.");
            Console.WriteLine("2. 더블 스트라이크 - MP 15");
            Console.WriteLine("공격력 * 1.5 로 두 명의 랜덤한 적을 공격합니다.");
            Console.WriteLine("0. 취소");
            string select = Console.ReadLine();
            if (select == "1")
            {
                //스킬 MP 
                AlphaStrike(player, monsters);
                player.MP -= 10;
            }
            else if (select == "2")
            {
                DoubleStrike(player, monsters);
                player.MP -= 15;
            }
            else
            {
                StartBattle(player);
            }
        }
        public void AlphaStrike(Player player, List<Monster> monsters)
        {
            Console.WriteLine("공격할 대상을 선택하시오");
            int select = int.Parse(Console.ReadLine()) - 1;
            int Attack = player.Attack * 2;
            Console.WriteLine("알파 스트라이크!");
            monsters[select].MonsterHP -= Attack;
            Console.WriteLine($"{monsters[select].MonsterName}에게 {Attack}만큼의 피해를 입혔습니다.");
            if (monsters[select].MonsterHP <= 0)
            {
                monsters[select].MonsterHP = 0; // HP가 0 아래로 내려가지 않도록 보정
                monsters[select].MonsterDie = true;

                Console.WriteLine("{0}이(가) 사망했습니다.", monsters[select].MonsterName);
                monsterDieCount--;
                player.DungeonClearCount++;
                if ((player.DungeonClearCount / 5) >= player.LV)  //레벨 오르는 부분
                {
                    player.LV++;
                    player.Attack += 1; // 공격력 증가
                    player.Defense += 2; // 방어력 증가
                    Console.WriteLine($"레벨이 올랐습니다. 현재 레벨: {player.LV}, 공격력: {player.Attack}, 방어력: {player.Defense}");
                }
                Console.WriteLine("몬스터를 처치하여 소량의 골드를 얻었습니다.");
                int gold = rand.Next(100, 1001);
                Console.WriteLine($"골드를 {gold}G 얻었습니다!");
                player.Gold += gold;
                quest.MonsterDies(player);
            }
            Console.ReadKey();
            EnemyPhase(player, monsters);

        }
        public void DoubleStrike(Player player, List<Monster> monsters)
        {
            Console.WriteLine("랜덤한 대상을 공격합니다.");
            double ranAttack = player.Attack * 1.5;
            Console.WriteLine("더블 스트라이크!");
            int SmonsterCount = monsterCount;
            for (int i = 1; i <= SmonsterCount; i++)
            {
                int j = rand.Next(0, SmonsterCount);
                Console.WriteLine();
                monsters[j].MonsterHP -= (int)ranAttack;
                Console.WriteLine($"{monsters[j].MonsterName}에게 {(int)ranAttack}만큼의 피해를 입혔습니다.");
                if (monsters[j].MonsterHP <= 0)
                {
                    monsters[j].MonsterHP = 0; // HP가 0 아래로 내려가지 않도록 보정
                    monsters[j].MonsterDie = true;

                    Console.WriteLine("{0}이(가) 사망했습니다.", monsters[j].MonsterName);
                    monsterDieCount--;
                    SmonsterCount--;
                    player.DungeonClearCount++;
                    if ((player.DungeonClearCount / 5) >= player.LV)  //레벨 오르는 부분
                    {
                        player.LV++;
                        player.Attack += 1; // 공격력 증가
                        player.Defense += 2; // 방어력 증가
                        Console.WriteLine($"레벨이 올랐습니다. 현재 레벨: {player.LV}, 공격력: {player.Attack}, 방어력: {player.Defense}");
                    }
                    Console.WriteLine("몬스터를 처치하여 소량의 골드를 얻었습니다.");
                    int gold = rand.Next(100, 1001);
                    Console.WriteLine($"골드를 {gold}G 얻었습니다!");
                    player.Gold += gold;
                    quest.MonsterDies(player);
                }
            }
            Console.ReadKey();
            EnemyPhase(player, monsters);

        }
        // 적 몬스터 공격 페이즈
        // 플레이어의 방어력을 몬스터의 공격력에서 빼서 데미지를 계산하고, 음수가 되면 데미지가 없도록 설정
        public void EnemyPhase(Player player, List<Monster> monsters)
        {
            Console.Clear();
            Console.WriteLine("\nEnemy Phase 시작\n");

            // 모든 몬스터가 죽은지 확인
            bool allMonstersDead = true;
            foreach (var monster in monsters)
            {
                if (!monster.MonsterDie)
                {
                    allMonstersDead = false;
                    break;
                }
            }

            if (allMonstersDead)
            {
                // 몬스터 카운트 초기화
                monsterCount = 0;

                // 기존에 존재했던 몬스터들을 전부 제거
                monsters.Clear();

                // 다시 몬스터 스폰
                StartBattle(player);
            }
            else
            {
                // 살아있는 몬스터에 대해 공격 수행
                foreach (var monster in monsters)
                {
                    if (!monster.MonsterDie)
                    {
                        int con = 0;
                        con = rand.Next(0, 100);
                        if (con > 70)
                        {
                            Console.WriteLine("회피하였습니다.");
                        }
                        else
                        {
                            // 몬스터 공격력 계산
                            int damage = CalculateDamage(monster.AttackMonster);

                            // 플레이어 방어력을 고려하여 데미지 계산
                            damage -= player.Defense;
                            if (damage < 0)
                            {
                                damage = 0; // 음수인 경우 데미지가 없도록 설정
                            }

                            // 플레이어 체력 감소
                            player.HP -= damage;

                            // 공격 결과 출력
                            Console.WriteLine("{0}의 공격!", monster.MonsterName);
                            Console.WriteLine("Chad을(를) 공격하여 {0}만큼의 데미지를 입혔습니다.", damage);
                            Console.WriteLine("Chad의 체력: {0}/{1}\n", player.HP, 100);
                        }
                        // 플레이어가 죽은 경우
                        if (player.HP <= 0)
                        {
                            Console.WriteLine("플레이어가 사망했습니다. 게임 오버!");
                            GameOver(player);
                            return;
                        }
                    }
                }
                // 다음 행동 선택
                Console.WriteLine("0. 도망치기");
                Console.WriteLine("1. 진행하기");
                string input = Console.ReadLine();
                if (input == "0")
                {
                    // 플레이어의 차례로 돌아가기
                    //ShowDungeon(player);
                }
                else
                {
                    StartBattle(player);
                    Console.ReadKey();
                }
            }
        }
        // 게임 오버 메서드
        public void GameOver(Player player)
        {
            Console.Clear();
            Console.WriteLine(@"  /$$$$$$                                           /$$$$$$                                      /$$       /$$
 /$$__  $$                                         /$$__  $$                                    | $$      | $$
| $$  \__/  /$$$$$$  /$$$$$$/$$$$   /$$$$$$       | $$  \ $$ /$$    /$$ /$$$$$$   /$$$$$$       | $$      | $$
| $$ /$$$$ |____  $$| $$_  $$_  $$ /$$__  $$      | $$  | $$|  $$  /$$//$$__  $$ /$$__  $$      | $$      | $$
| $$|_  $$  /$$$$$$$| $$ \ $$ \ $$| $$$$$$$$      | $$  | $$ \  $$/$$/| $$$$$$$$| $$  \__/      |__/      |__/
| $$  \ $$ /$$__  $$| $$ | $$ | $$| $$_____/      | $$  | $$  \  $$$/ | $$_____/| $$                          
|  $$$$$$/|  $$$$$$$| $$ | $$ | $$|  $$$$$$$      |  $$$$$$/   \  $/  |  $$$$$$$| $$             /$$       /$$
 \______/  \_______/|__/ |__/ |__/ \_______/       \______/     \_/    \_______/|__/            |__/      |__/
                                                                                                              
                                                                                                              
                                                                                                              ");
            Console.WriteLine("모든 돈을 잃어버렸습니다...");
            player.Gold = 0;
            player.HP = 100; // 체력 초기화
            Console.WriteLine("아무 키나 눌러 던전으로 돌아가기");
            Console.ReadKey();
        }
        public void GameClear()
        {
            Console.Clear();
            Console.WriteLine(@"
      ::::::::           :::          :::   :::       ::::::::::        ::::::::       :::        ::::::::::           :::        :::::::::         :::::   :::       :::       ::: 
    :+:    :+:        :+: :+:       :+:+: :+:+:      :+:              :+:    :+:      :+:        :+:                :+: :+:      :+:    :+:      :+:   :+:+:         :+:       :+:  
   +:+              +:+   +:+     +:+ +:+:+ +:+     +:+              +:+             +:+        +:+               +:+   +:+     +:+    +:+                          +:+       +:+   
  :#:             +#++:++#++:    +#+  +:+  +#+     +#++:++#         +#+             +#+        +#++:++#         +#++:++#++:    +#++:++#:                           +#+       +#+    
 +#+   +#+#      +#+     +#+    +#+       +#+     +#+              +#+             +#+        +#+              +#+     +#+    +#+    +#+                          +#+       +#+     
#+#    #+#      #+#     #+#    #+#       #+#     #+#              #+#    #+#      #+#        #+#              #+#     #+#    #+#    #+#                                             
########       ###     ###    ###       ###     ##########        ########       ########## ##########       ###     ###    ###    ###                          ###       ###       
");
            Console.WriteLine("축하합니다. 클리어하셨습니다.");
            Console.WriteLine("아무 키나 눌러 던전으로 돌아가기");
            Console.ReadKey();
        }
    }
}
