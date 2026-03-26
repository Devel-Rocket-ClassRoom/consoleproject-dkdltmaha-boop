using DungeonGame2;
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
        static Random rnd = new Random();

        public int hp = 100;
        public int r;
        public int c;
        public int rush = 0;

        public void rushAttk(Player player)                                             //공격 선언시 랜덤 성공 or 회피 발동 
        {
            int dice = rnd.Next(1, 7);

            switch (dice)
            {
                case 1:
                    Console.WriteLine(" 빗나감 ");
                    rush = 0;
                    break;
                case 2:
                    Console.WriteLine(" 약한 공격 ");
                    rush = 10;
                    break;
                case 3:
                    Console.WriteLine(" 공격 ");
                    rush = 20;
                    break;
                case 4:
                    Console.WriteLine("강한 공격");
                    rush = 30;
                    break;
                case 5:
                    Console.WriteLine(" 큰 피해를 입힘 ");
                    rush = 40;
                    break;
                case 6:
                    Console.WriteLine(" 치명타! ");
                    rush = 50;
                    break;
            }
            player.hp -= rush;
            Console.WriteLine($" 플레이어hp가 {player.hp} 남았습니다 ");
        }
    }


    public class Player  //플레이어 클래스 생성
    {
        static Random rnd = new Random();


        public int power = 0;
        public int pr;
        public int pc;
        public int hp = 200;



        public void attack(Monster monster)     // 플레이어 공격 함수
        {
            int dice = rnd.Next(1, 7);

            switch (dice)
            {
                case 1:
                    Console.WriteLine(" 빗나감 ");
                    power = 0;
                    break;
                case 2:
                    Console.WriteLine(" 약한 공격 ");
                    power = 20;
                    break;
                case 3:
                    Console.WriteLine(" 공격 ");
                    power = 30;
                    break;
                case 4:
                    Console.WriteLine("강한 공격");
                    power = 40;
                    break;
                case 5:
                    Console.WriteLine(" 큰 피해를 입힘 ");
                    power = 50;
                    break;
                case 6:
                    Console.WriteLine(" 치명타! ");
                    power = 100;
                    break;
            }
            monster.hp -= power;
            Console.WriteLine($" 몬스터hp가 {monster.hp} 남았습니다 ");
        }
    }

    public class Map
    {

        int rows;
        int cols;

        public char[,] map =   ///// 맵을 생성 (생성만 하고 출력 X)
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
            var mm = SaveLoadJson.ConvertMap(stage.map);

            SaveLoadJson.SaveGameData(mm);

            var pos = stage.FindPlayerPosition();
            player.pr = pos.Item1;
            player.pc = pos.Item2;

            

            for (int r = 0; r < stage.GetRows(); r++)
            {
                for (int c = 0; c < stage.GetCols(); c++)
                {
                    if (stage.GetTile(r, c) == 'M')
                    {
                        Monster m = new Monster();
                        m.r = r;
                        m.c = c;
                        monsters.Add(m);
                    }
                }
            }

            while (true)
            {
                Console.Clear();
                stage.PrintMap();

                Console.Write("이동: ");
                string cmd = Console.ReadLine();

                if (cmd == null) continue;

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

                char Turn = 'P';


                for (int i = 0; i < monsters.Count; i++)
                {
                    Monster m = monsters[i];

                    int dist = Math.Abs(player.pr - m.r) + Math.Abs(player.pc - m.c);

                    if (dist == 1)
                    {
                        Console.WriteLine("전투 시작!");

                        while (player.hp > 0 && m.hp > 0)      // player 또는 monster가 hp가 0이 될때까지 반복
                        {
                            if (Turn == 'P')
                            {
                                string input = "Q";

                                if (input == "Q")
                                {
                                    player.attack(m);
                                    Turn = 'M';
                                }
                            }
                            else
                            {
                                Console.WriteLine("몬스터 턴");
                                m.rushAttk(player);
                                Turn = 'P';
                                Console.ReadLine();
                            }

                        }
                        if (player.hp <= 0)
                        { 
                            Console.WriteLine("플레이어 패배");
                            return;
                        }
                        else if (m.hp <= 0)
                        {
                            monsters.RemoveAt(i);

                            stage.SetTile(m.r, m.c, ' ');
                            i--;
                            Console.WriteLine("몬스터 처치");
                        }
                    }

                }

            }

        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        DungeonGame game = new DungeonGame(); //던전 게임 객체 생성


        game.Gameplay(); //게임 시작


    }
}









