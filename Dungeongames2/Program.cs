using System.Text.Json;
using System.Text.Json.Serialization;

namespace DungeonGame2
{
    public class SaveLoadJson
    {

        // 2차원 맵을 char[][]로 바꾸는 함수
        static public char[][] ConvertMap(char[,] map)
        {
            
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            char[][] result = new char[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new char[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = map[i, j];
                }
            }

            return result;
        }

        // GameData를 Json으로 저장하는 테스트 함수
        static public void SaveGameData(char[][] m)
        {
            
            // 저장할 파일 경로
            string folderPath = "./GameData";
            string filePath = Path.Combine(folderPath, "data.json");  // 폴더와 파일 이름 합치기

            // 폴더가 존재하지 않을 경우 생성
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 직렬화
            string result = JsonSerializer.Serialize(m, new JsonSerializerOptions { WriteIndented = false });
            File.WriteAllText(filePath, result);

            // 테스트 출력
            Console.WriteLine(result);
        }

        // Json으로 저장된 GameData를 읽는 테스트 함수
        public void LoadGameData()
        {
            string folderPath = "./GameData";
            string filePath = Path.Combine(folderPath, "data.json");  // 폴더와 파일 이름 합치기

            // 역직렬화
            string s = File.ReadAllText(filePath);
            GameData mm = JsonSerializer.Deserialize<GameData>(s);

            if (mm != null)
            {
                Console.WriteLine("읽기 성공!: " + mm);
            }
            else
            {
                Console.WriteLine("정상적인 데이터가 아닙니다.");
            }
        }

        public class GameData
        {
            // Json에 포함
            [JsonInclude] private string stageName;
            [JsonInclude] private int dungeonCount;

            // 포함하지 않음
            [JsonIgnore] public int Count { get; set; }

            public GameData(string name, int dCount, int count)
            {
                stageName = name;
                dungeonCount = dCount;
                Count = count;
            }
        }
    }
}