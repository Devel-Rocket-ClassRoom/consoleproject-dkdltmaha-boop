using DungeonGames2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static DungeonGames2.Player;

namespace DungeonGames2
{

    public struct GameData // 스테이지 정보 저장 구조체
    {
        public string stageName; // 스테이지 이름 저장
        public int dungeonCount; // 몬스터 개수 정보

        public GameData(string name, int count) // 게임 데이터 만들때
        {
            stageName = name;    //받은 값을 변수에 저장
            dungeonCount = count;
        }
    }



    public class Monster   //몬스터 클래스 생성
    {
        Random rnd = new Random();

        public int hp = 100;
        public int r;
        public int c;
        public int rush = 30;

        public void rushAttk(Player player, Map map)                                             //공격 선언시 랜덤 성공 or 회피 발동 
        {
            int dice = rnd.Next(1, 7);

            if (dice <= 2)
            {
                Console.WriteLine("공격을 회피하였습니다")
                return;
            }
            Console.WriteLine("공격을 당했습니다")
            player.hp -= rush;
        }
    }


    public class Player  //플레이어 클래스 생성
    {
        Random rnd = new Random();

        public int power = 50;
        public int pr;
        public int pc;
        public int hp = 200;



        public void attack(Monster monster, Map map)     // 플레이어 공격 함수
        {
            Random rnd = new Random();

            int dice = rnd.Next(1, 7);

            if (dice <= 2)
            {
                Console.WriteLine("몬스터가 공격을 회피하였습니다")
                return;              
            }
            Console.WriteLine("공격을 성공하였습니다")
            monster.hp -= power;
        }


        public class Map
        {

            int rows;
            int cols;

            char[,] map =   ///// 맵을 생성 (생성만 하고 출력 X)
             {
            { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' },
            { '#', 'P', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'M', ' ', ' ', ' ', ' ', '#' },
            { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
            { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'M', ' ', ' ', ' ', ' ', '#' },
            { '#', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
            { '#', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
            { '#', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
            { '#', ' ', 'M', ' ', ' ', ' ', 'M', ' ', 'M', ' ', ' ', ' ', ' ', ' ', '#' },
            { '#', ' ', ' ', 'M', ' ', ' ', ' ', 'M', ' ', ' ', ' ', ' ', ' ', 'M', '#' },
            { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' } };

            public Map()
            {
                rows = map.GetLength(0);
                cols = map.GetLength(1);
            }
            public int GetRows() => rows;
            public int GetCols() => cols;


            public char GetTile(int r, int c)
            {
                return map[r, c];
            }


            public void SetTile(int r, int c, char value)
            {
                map[r, c] = value;
            }


            public void PrintMap()  //맵 출력 함수
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        Console.Write(map[r, c]); //맵 문자 출력
                    }
                    Console.WriteLine();
                }
            }

        }
        public (int, int) FindPlayerPosition() //맵 전체 탐색
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (map[r, c] == 'P') //플레이어 찾으면
                    {
                        return (r, c); //좌표 반환
                    }
                }
            }
            return (-1, -1);
        }


        public int CountMonster() /// 몬스터 개수 찾기
        {
            int count = 0;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (map[r, c] == 'M')
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public bool IsMonster(int r, int c) //bool 'M' 위치가 R,C가 맞으면 true값을 출력 아닐시 false
        {
            if (map[r, c] == 'M')
            {
                return true;
            }
            return false;
        }
    }
    public class DungeonGame // 던전게임 클래스 생성
    {
        int rows; // 맵크기 저장
        int cols;
        Map stage = new Map();
        Player player = new Player();
        int monsterCount; // 몬스터 수 저장

        List<Monster> monsters = new List<Monster>();

        List<GameData> stages = new List<GameData>();

        Dictionary<string, (int, int)> moveDir = new Dictionary<string, (int, int)>()
            {
                { "L", (0, -1) },
                { "R", (0, 1) },
                { "U", (-1, 0) },
                { "D", (1, 0) } };



        public void Gameplay() //게임 진행   입력,이동,몬스터 처리
        {

            while (true)
            {
                if (!moveDir.ContainsKey(cmd)) continue;

                int R = player.pr + moveDir[cmd].Item1;
                int C = player.pc + moveDir[cmd].Item2;

                if (stage.GetTile(R, C) == '#')
                {
                    Console.WriteLine("이동 불가");
                    continue;
                }

                stage.SetTile(player.pr, player.pc, ' ');
                player.pr = R;
                player.pc = C;
                stage.SetTile(player.pr, player.pc, 'P');




                monsterCount = stage.CountMonster();
                stages.Add(new GameData("Stage 1", monsterCount));

                (int, int) pos = stage.FindPlayerPosition();
                player.pr = pos.Item1;
                player.pc = pos.Item2;




                while (true)    //게임 계속 반복
                {
                    Console.Clear(); //화면 초기화
                    Console.WriteLine($"[{stages[0].stageName}] 남은 몬스터: {monsterCount}");
                    stage.PrintMap();  //현재 맵 상태 출력


                    string cmd = Console.ReadLine();  //플레이어 입력

                    char currentPlayer = player;

                    if (currentPlayer == player)
                    {
                        currentPlayer = monster;
                    }
                    else
                    {
                        currentPlayer = player;
                    }

                }
                for (int i = 0; i < monsters.Count; i++)
                {
                    Monster m = monsters[i];//꺼낸 몬스터를 m이라는 이름으로 사용

                    int dist = Math.Abs(player.pr - m.r) + Math.Abs(player.pc - m.c);// dist = distance (플레이어와 몬스터 사이 거리 저장 변수)  
                                                                                     //맞으면 Attack
                    if (dist == 1)
                    {
                        while (true)
                        {
                            player.attack(m, stage);
                            if (m.hp <= 0)
                            {
                                Console.WriteLine("몬스터 처치!!!");
                            }
                            if (player.hp <= 0)
                                    {
                                Console.WriteLine("사망하였습니다..");
                            }
                        }
                    }
                }


            }
        }
    }




    class program
    {
        static void Main(string[] args)
        {
            DungeonGame game = new DungeonGame(); //던전 게임 객체 생성
            game.Gameplay(); //게임 시작
        }
    }






