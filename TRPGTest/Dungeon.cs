using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using static TRPGTest.Dungeon;

namespace TRPGTest
{
    internal class Dungeon
    {
        // 몬스터 클래스
        public class Monster
        {
            public string MonsterName { get; set; } // 몬스터 이름
            public int MonsterLV { get; set; } // 몬스터 레벨
            public int MonsterHP { get; set; } // 몬스터 체력
            public int AttackMonster { get; set; } // 몬스터 공격력
            public bool MonsterDie { get; set; } // 몬스터 생사구분
        }

        // 도망가기 커맨드 여부 판별
        public enum Act
        {
            Fight,
            Run
        }

        string input = "";
        int target1;
        int target2;
        public int monsterDieCount = 0;
        public int stage = 0;
        public int monsterCount = 0;
        List<Monster> monsters; // 몬스터 리스트
        public Random rand = new Random();
        QuestManager quest;
        // 랜덤 몬스터 생성 메서드
        public Monster CreateRandomMonster()
        {
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


        // 던전 입장 화면
        public void ShowDungeon(Player player)
        {
            while (input != "0")
            {
                input = "";
                Console.Clear();

                Console.WriteLine("던전");
                Console.WriteLine("전투로 골드를 얻을 수 있는 던전입니다.\n");
                Console.WriteLine($"현재 체력: {player.HP}");
                Console.WriteLine($"보유 골드: {player.Gold} G\n");

                Console.WriteLine("1. 전투 시작");
                Console.WriteLine("0. 나가기\n");

                while (input != "0" && input != "1")
                {
                    Console.Write("원하시는 행동을 입력해주세요.\n> ");
                    input = Console.ReadLine();

                    if (input == "1")
                    {
                        StartBattle(player);  // 전투 시작
                        input = "1";
                    }
                    if (input == "0")
                    {
                        input = "";
                        return;
                    }
                    else
                        Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }


        // 전투 시작 화면
        public void StartBattle(Player player)
        {
            while (input != "0")  // 플레이어 행동 입력
            {
                input = "";
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
                        monsterCount = rand.Next(1, 5);
                        Console.WriteLine($"총 {monsterCount}마리의 몬스터가 나타났습니다!");

                        // 랜덤 몬스터 생성
                        for (int i = 0; i < monsterCount; i++)
                        {
                            monsters.Add(CreateRandomMonster());
                            monsterDieCount++;
                        }
                    }
                }

                // 플레이어 정보 출력
                Console.WriteLine("\n[캐릭터 정보]");
                Console.WriteLine($"Lv.{player.LV} {player.Name} ({player.Job})\n");
                Console.WriteLine($"현재 HP: {player.HP} / {100}");
                Console.WriteLine($"남은 MP: {player.MP} / {50}\n");
                Console.WriteLine($"[보유 골드] {player.Gold} G\n");
                Console.WriteLine($"현재 던전의 {stage}층입니다.\n");

                // 몬스터 정보 출력
                Console.WriteLine("[몬스터]");
                for (int i = 0; i < monsterCount; i++)
                {
                    // 간혹 체력이 0 미만인데도 죽지 않은 몬스터가 보이는데, 아마 진행에 지장 없이 한 대 때리면 죽으니 확률성 이스터에그처럼 땜빵합니다.
                    string monsterStatus = monsters[i].MonsterHP < 0 ? " (유령)" : "";

                    Console.WriteLine($"{i + 1}. Lv.{monsters[i].MonsterLV} {monsters[i].MonsterName}{monsterStatus} | HP {monsters[i].MonsterHP}");
                }

                Console.WriteLine("\n1. 공격");
                Console.WriteLine("2. 스킬");
                // Console.WriteLine("3. 다음 층으로");
                Console.WriteLine("0. 도망치기 (공격 회피율 상승)\n");

                while (input != "0" && input != "1" && input != "2")  // 플레이어 행동 입력
                {
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write("> ");
                    input = Console.ReadLine();

                    if (input == "1")
                    {
                        AttackMonster(player, monsters);
                    }
                    else if (input == "2")
                    {
                        Skill(player, monsters);
                    }
                    else if (input == "0")
                        EnemyPhase(player, monsters, Act.Run);
                    else
                        Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }


        // 몬스터 선택 메서드
        public int MonsterSelect()
        {
            Console.WriteLine("공격하실 대상을 선택해주세요.");
            Console.Write("0. 취소\n> ");
            int targetIndex;

            // 입력이 유효할 때까지 반복
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out targetIndex) || targetIndex < 0 || targetIndex > monsters.Count)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine("공격하실 대상을 선택해주세요.");
                    Console.Write("0. 취소\n> ");
                }
                else if (targetIndex == 0)
                    return -1;
                else if (monsters[targetIndex - 1].MonsterDie)  // 선택한 몬스터가 이미 죽은 경우
                {
                    Console.WriteLine("이미 죽은 몬스터를 공격할 수 없습니다.");
                    Console.WriteLine("공격하실 대상을 선택해주세요.");
                    Console.Write("0. 취소\n> ");
                }
                else
                    return targetIndex - 1;
            }
        }


        // 공격 메서드
        public void AttackMonster(Player player, List<Monster> monsters)
        {
            // 선택한 몬스터 인덱스 계산 (0부터 시작하는 인덱스로 변환)
            int monsterIndex = MonsterSelect();
            if (monsterIndex < 0)
                return;

            // 몬스터 공격 if 문으로 치명타를 설정
            int criticalProbability = rand.Next(1, 11);  // 치명타 확률

            if (criticalProbability > 7)
            {
                Console.WriteLine("치명타 발동!");
                int criticalDamage = CriticalHit(player.Attack);
                monsters[monsterIndex].MonsterHP -= criticalDamage;

                Console.WriteLine($"\n{player.Name}의 공격!");  // 공격 결과 출력
                Console.WriteLine($"{monsters[monsterIndex].MonsterName}에게 {criticalDamage}만큼의 피해를 입혔습니다.");
            }
            else
            {
                int damage = CalculateDamage(player.Attack);
                monsters[monsterIndex].MonsterHP -= damage;

                Console.WriteLine($"\n{player.Name}의 공격!");  // 공격 결과 출력
                Console.WriteLine($"{monsters[monsterIndex].MonsterName}에게 {damage}만큼의 피해를 입혔습니다.");
            }
            Console.ReadKey(true);

            // 몬스터가 죽은 경우 체크
            if (monsters[monsterIndex].MonsterHP <= 0)
            {
                MonsterDead(player, monsterIndex);
            }
            EnemyPhase(player, monsters, Act.Fight);
        }


        // 대미지 계산 메서드 (10% 오차 적용)
        public int CalculateDamage(int attack)
        {
            double error = rand.NextDouble() * 0.2 - 0.1;  // -0.1부터 0.1까지의 오차
            int finalDamage = (int)Math.Ceiling(attack * (1 + error));  // 최종 대미지 계산 (오차 적용)
            return finalDamage;
        }


        // 치명타 대미지 계산 메서드
        public int CriticalHit(int attack)
        {
            double error = rand.NextDouble() * 0.2 - 0.1;  // -0.1부터 0.1까지의 오차
            int finalDamage = (int)Math.Ceiling(attack * (1.5 + error));  // 최종 공격력 계산 (오차 적용)
            return finalDamage;

            // 대미지 계산 메서드 안에서 치명타 여부를 판별할 때 사용
            /*
            int critical = new Random().Next(1, 100);
            int criticalDamage = 0;

            bool isCritical = false;
            if (critical >= 15)
            {
                isCritical = true;
                double newCriticalattack = attack * 1.6;
                criticalDamage = (int)Math.Round(newCriticalattack);
            }
            else
            {
                isCritical = false;
            }
            int intCriticalDamage = (int)criticalDamage;
            return intCriticalDamage;
            */
        }


        // 스킬 선택지 메서드
        public void Skill(Player player, List<Monster> monsters)
        {
            Console.WriteLine("[스킬 목록]");
            Console.WriteLine("1. 알파 스트라이크 - MP 10");
            Console.WriteLine("공격력의 2배 위력으로 하나의 적을 공격합니다.");
            Console.WriteLine("2. 더블 스트라이크 - MP 15");
            Console.WriteLine("공격력의 1.5배 위력으로 랜덤한 적 둘을 공격합니다.");
            Console.WriteLine("0. 스킬 사용 취소");

            string select = "";

            while (select != "0" && select != "1" && select != "2")  // 플레이어 행동 입력
            {
                Console.WriteLine("사용하실 스킬을 선택해주세요.");
                Console.Write("> ");
                select = Console.ReadLine();

                switch (select)
                {
                    case "1":
                        if (player.MP < 10)
                        {
                            Console.WriteLine("MP가 부족합니다.");
                            select = "";
                        }
                        else
                            AlphaStrike(player, monsters);
                        break;
                    case "2":
                        if (player.MP < 15)
                        {
                            Console.WriteLine("MP가 부족합니다.");
                            select = "";
                        }
                        else
                            DoubleStrike(player, monsters);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }


        // 스킬 - 알파 스트라이크 메서드
        public void AlphaStrike(Player player, List<Monster> monsters)
        {
            // 선택한 몬스터 인덱스 계산 (0부터 시작하는 인덱스로 변환)
            int select = MonsterSelect();
            if (select < 0)
                return;

            // 스킬 MP 소모, 공격 진행
            player.MP -= 10;
            int Attack = player.Attack * 2;
            monsters[select].MonsterHP -= Attack;

            Console.WriteLine($"\n{player.Name}의 알파 스트라이크!");
            Console.WriteLine($"{monsters[select].MonsterName}에게 {Attack}만큼의 피해를 입혔습니다.");
            Console.ReadKey(true);

            // 몬스터가 죽은 경우 체크
            if (monsters[select].MonsterHP <= 0)
            {
                MonsterDead(player, select);
            }
            EnemyPhase(player, monsters, Act.Fight);
        }


        // 스킬 - 더블 스트라이크 메서드
        public void DoubleStrike(Player player, List<Monster> monsters)
        {
            // 스킬 MP 소모, 공격 진행
            player.MP -= 15;
            double dsAttack = player.Attack * 1.5;

            // 죽지 않은 몬스터만 대상으로 지정
            List<Monster> dsMonsters = new List<Monster>();
            Dictionary<int, int> listMatch = new Dictionary<int, int>();  // Key - dsMonsters 인덱스, Value - monsters 인덱스
            int dsCount = 0;

            for (int i = 0; i < monsterCount; i++)
            {
                if (!monsters[i].MonsterDie)
                {
                    dsMonsters.Add(monsters[i]);
                    listMatch.Add(dsCount, i);
                    dsCount++;
                }
            }

            // 공격 진행
            Console.WriteLine("랜덤한 대상 둘을 공격합니다.");
            Console.WriteLine($"\n{player.Name}의 더블 스트라이크!");

            for (int i = 0; i < 2; i++)
            {
                int j = rand.Next(0, monsterDieCount);

                dsMonsters[j].MonsterHP -= (int)dsAttack;
                Console.WriteLine($"{dsMonsters[j].MonsterName}에게 {(int)dsAttack}만큼의 피해를 입혔습니다.");
                Console.ReadKey(true);

                // 몬스터가 죽은 경우 체크
                if (dsMonsters[j].MonsterHP <= 0)
                {
                    // 여기에 dsMonsters 가 아니라 monsters 리스트의 인덱스를 넣기 위해 Dictionary 사용
                    MonsterDead(player, listMatch[j]);
                    dsMonsters.Remove(dsMonsters[j]);  // 이미 죽은 몬스터를 공격하지 않도록
                }

                if (monsterDieCount < 1)  // 도중에 몬스터를 전부 처치했을 경우 중단
                    break;
            }
            EnemyPhase(player, monsters, Act.Fight);
        }


        // 몬스터가 죽은 경우 체크하는 메서드
        public void MonsterDead(Player player, int monsterIndex)
        {
            monsters[monsterIndex].MonsterHP = 0;  // HP가 0 아래로 내려가지 않도록 보정
            monsters[monsterIndex].MonsterDie = true;

            Console.WriteLine($"{monsters[monsterIndex].MonsterName}을(를) 쓰러트렸습니다!");
            monsterDieCount--;
            player.DungeonClearCount++;

            // 골드 획득
            int gold = rand.Next(100, 1001);
            Console.WriteLine($"몬스터를 처치하여 {gold} G 를 얻었습니다.");
            player.Gold += gold;

            // 캐릭터 레벨업
            if ((player.DungeonClearCount / 5) >= player.LV)
            {
                player.LV++;
                player.Attack += 1; // 공격력 증가
                player.Defense += 2; // 방어력 증가
                quest.LerverUp(player);
                Console.WriteLine($"{player.Name}의 레벨이 올랐습니다! [현재 레벨: {player.LV}, 공격력: {player.Attack}, 방어력: {player.Defense}]");
            }

            // 퀘스트 체크
            quest.MonsterDies(player);

            Console.ReadKey(true);
        }


        // 적 몬스터 공격 페이즈
        // 몬스터 공격력 - 플레이어 방어력 = 대미지, 음수가 되면 데미지 없음
        public void EnemyPhase(Player player, List<Monster> monsters, Act act)
        {
            input = "";
            Console.Clear();

            // 모든 몬스터가 죽었는지 확인
            bool allMonstersDead = true;
            foreach (var monster in monsters)
            {
                if (!monster.MonsterDie)
                {
                    allMonstersDead = false;
                    break;
                }
            }

            // 모든 몬스터가 죽었을 경우
            if (allMonstersDead)
            {
                // 몬스터 카운트 초기화
                monsterCount = 0;
                // 기존에 존재했던 몬스터들을 전부 제거
                monsters.Clear();

                if (stage == 15)
                {
                    GameClear();
                    return;
                }

                Console.WriteLine("**Clear!**\n");
                Console.WriteLine($"{stage}층의 몬스터를 모두 처치했습니다.\n");

                Console.WriteLine("1. 다음 층으로");
                Console.WriteLine("0. 나가기\n");

                while (input != "0" && input != "1")  // 플레이어 행동 입력
                {
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write("> ");
                    input = Console.ReadLine();

                    if (input == "1")
                        return;
                    else if (input == "0")
                        return;
                    else
                        Console.WriteLine("잘못된 입력입니다.");
                }
            }
            // 살아 있는 몬스터가 존재할 경우
            else
            {
                Console.WriteLine("**Enemy Phase 시작**\n");

                // 살아있는 몬스터에 대해 공격 수행
                foreach (var monster in monsters)
                {
                    if (!monster.MonsterDie)
                    {
                        Console.WriteLine($"{monster.MonsterName}의 공격!");

                        int con = rand.Next(0, 100);
                        int avoid = 70;  // 기본 공격 회피 확률 30%

                        if (act == Act.Run)  // 도망칠 때 공격 회피 확률 70%
                            avoid = 30;

                        if (con > avoid)
                        {
                            Console.WriteLine($"{monster.MonsterName}의 공격을 회피했습니다!\n");
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
                            Console.WriteLine($"{player.Name}은(는) {damage}만큼의 피해를 입었습니다.");
                            Console.WriteLine($"현재 체력: {player.HP} / {100}\n");
                        }

                        // 플레이어가 죽은 경우
                        if (player.HP <= 0)
                        {
                            Console.WriteLine("체력을 전부 소진했습니다...");
                            Console.ReadKey(true);
                            GameOver(player);
                            return;
                        }
                    }
                }

                if (act == Act.Run)  // 도망치기를 선택했을 시
                {
                    Console.WriteLine("던전에서 도망칩니다.");
                    input = "0";
                    Console.ReadKey(true);
                    return;
                }

                Console.WriteLine("1. 계속 진행하기");
                Console.WriteLine("0. 도망치기\n");

                while (input != "0" && input != "1")  // 플레이어 행동 입력
                {
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write("> ");
                    input = Console.ReadLine();

                    if (input == "1")
                        return;
                    else if (input == "0")
                    {
                        Console.WriteLine("던전에서 도망칩니다.");
                        Console.ReadKey(true);
                        return;
                    }
                    else
                        Console.WriteLine("잘못된 입력입니다.");
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
            Console.WriteLine("모든 돈을 잃어버렸습니다...\n");

            player.Gold = 0;
            player.HP = 100; // 체력 초기화
            input = "0";
            Console.WriteLine("아무 키나 누르시면 던전 입구로 돌아갑니다.");
            Console.ReadKey(true);
        }


        // 게임 클리어 메서드
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
            Console.WriteLine("축하합니다, 스파르타 던전을 클리어하셨습니다!");
            Console.WriteLine("당신은 던전의 끝에서 '플레이해 주셔서 감사합니다. code: allmaster'라는 글귀를 찾았습니다.\n");

            input = "0";
            Console.WriteLine("아무 키나 누르시면 던전 입구로 돌아갑니다.");
            Console.ReadKey(true);
        }
    }
}
