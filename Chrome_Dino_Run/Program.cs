using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ConsoleApp2
{
    using System.Threading;

    internal class Program
    {
        public static int highScore = 0; // 최고 기록
        public static int currentScore = 0; // 현재 기록
        public static bool quitGame = false; // 게임을 종료할 것인가
        public static bool footToggle = false; // 공룡 그리기가 갱신될 때 발의 위치를 바꿈
        public static bool isJumpping = false; // 현재 공룡이 점프 중인가

        static void Main(string[] args)
        {
            CursorSettings(); // 커서 정리
            GameStart(); // 게임 시작

            while (!quitGame)
            {
                ReStart(); // 게임 재시작
            }

        }

        static void GameStart()
        {
            ConsoleKeyInfo curKey; // 현재 입력 중인 키
            currentScore = 0; // 점수 초기화

            int yPos = 0; // 공룡의 y위치
            int max_Jump = 7; // 공룡의 최대 점프 위치

            int tree_Start = 235; // 장애물이 생성되는 위치
            int tree_xPos = tree_Start; // 장애물의 시작 x위치
            int tree_Collision = 7; // 장애물이 공룡과 충돌 가능한 X축 위치
            int y_Collision = 4; // y축의 충돌 기준위치
            int tree_End = -6; // 장애물이 사라지는 위치

            isJumpping = false;
            bool isJumpped = false; // 현재 공룡이 점프하여 최고 지점에 올랐는가
            bool isCollision = false; // 현재 공룡이 나무와 충돌하였는가
            bool isfloat = false; // 공중에 머무르는가

            while (true)
            {
                // 점수 출력 함수
                DrawScore(currentScore);

                // 키보드 입력이 있는지 체크  
                if (Console.KeyAvailable)
                {
                    // 키 입력 확인
                    curKey = GetKeyDown();

                    switch (curKey.Key)
                    {
                        case ConsoleKey.Escape:
                            isCollision = true;
                            quitGame = true;
                            break;
                        case ConsoleKey.Spacebar:
                            isJumpping = true;
                            break;
                        default:
                            break;
                    }
                }

                // 점프 관련
                // 점프 중인가
                if (isJumpping)
                {
                    // 최고 지점에 이르지 않았다면
                    if (yPos < max_Jump &&
                        isJumpped == false)
                        yPos++;
                    // 최고 지점에 도달 후 점프가 끝났다면
                    else if (isJumpped && yPos == 0)
                    {
                        isJumpped = false;
                        isJumpping = false;
                        isfloat = false;
                    }
                    // 최고 지점에 도달 후 잠깐 떠 있는 시간
                    else if (isJumpped && !isfloat)
                        isfloat = true;
                    // 최고 지점에 도달 후라면 (중력을 표현)
                    else if (isJumpped && isfloat)
                        yPos--;
                    // 최고 지점에 도달했다면
                    else if (yPos == max_Jump)
                        isJumpped = true;
                }
                // 점프 중이 아니라면
                else
                {
                    if (yPos > 0)
                        yPos--;
                }

                // 나무 위치 관련
                if (tree_xPos > tree_End)
                    tree_xPos -= 2 + (currentScore / 50);
                else
                    tree_xPos = tree_Start;

                // 충돌 관련
                // 나무의 X위치가 충돌 가능 X위치라면
                if (tree_xPos < tree_Collision)
                {
                    // 공룡의 Y위치가 충돌 가능 위치이고
                    // 나무의 X위치가 충돌 가능 위치라면
                    if (yPos < y_Collision &&
                        tree_xPos > tree_End + 1)
                        isCollision = true;
                }


                // 장애물 그리기 함수
                DrawTree(tree_xPos);

                // 공룡 그리기 함수
                DrawDino(yPos);

                Thread.Sleep(35); // 게임 딜레이
                Console.Clear(); // 화면 지우기

                // 충돌 시 게임오버
                if (isCollision)
                {
                    LoadDate();

                    if (currentScore > highScore)
                    {
                        highScore = currentScore;
                    }


                    Console.WriteLine("\n");
                    Console.WriteLine("\n");
                    Console.WriteLine("            Game Over");
                    Console.WriteLine("");
                    Console.WriteLine("         Best Score : " + highScore);
                    Console.WriteLine("           Score : " + currentScore);
                    Console.WriteLine("");
                    if (!quitGame)
                    {
                        Console.WriteLine("   재시작 하려면 R 키를 누르세요");
                    }

                    SaveData();

                    break;

                }
                // 충돌 상태가 아닐 때는 점수 증가
                else
                {
                    currentScore += 1;
                }
            }

        }

        static ConsoleKeyInfo GetKeyDown()
        {

            ConsoleKeyInfo cki;

            cki = Console.ReadKey(true);

            return cki;


        }

        static void DrawScore(int score)
        {
            Console.SetCursorPosition(110, 5);
            Console.WriteLine("Score : " + score);
        }

        static void DrawDino(int y)
        {
            int y_Base = 10;            // 공룡의 초기 Y축 위치
            Console.SetCursorPosition(0, y_Base - y);       // 공룡의 그리기 위치 변경

            Console.WriteLine("    ■■");
            Console.WriteLine("    ■");
            Console.WriteLine("■■■■");

            if (!isJumpping)
            {
                if (footToggle)
                {
                    Console.WriteLine("  ■");
                    footToggle = false;
                }
                else if (!footToggle)
                {
                    Console.WriteLine("    ■");
                    footToggle = true;
                }
            }
            else
            {
                Console.Write("   ■");
            }
        }

        static void DrawTree(int x)
        {
            int y_Base = 10;            // 공룡의 초기 Y축 위치

            if (x >= 0)
            {
                Console.SetCursorPosition(x, y_Base);
                Console.WriteLine("    ■");
                Console.SetCursorPosition(x, y_Base + 1);
                Console.WriteLine("■  ■");
                Console.SetCursorPosition(x, y_Base + 2);
                Console.WriteLine("■■■");
                Console.SetCursorPosition(x, y_Base + 3);
                Console.WriteLine("  ■");
            }
            else if (x >= -2)
            {
                x = 0;
                Console.SetCursorPosition(x, y_Base);
                Console.WriteLine("  ■");
                Console.SetCursorPosition(x, y_Base + 1);
                Console.WriteLine("  ■");
                Console.SetCursorPosition(x, y_Base + 2);
                Console.WriteLine("■■");
                Console.SetCursorPosition(x, y_Base + 3);
                Console.WriteLine("■");
            }
            else if (x >= -4)
            {
                x = 0;
                Console.SetCursorPosition(x, y_Base);
                Console.WriteLine("■");
                Console.SetCursorPosition(x, y_Base + 1);
                Console.WriteLine("■");
                Console.SetCursorPosition(x, y_Base + 2);
                Console.WriteLine("■");
                Console.SetCursorPosition(x, y_Base + 3);
                Console.WriteLine("");
            }
        }

        static void CursorSettings()
        {

            Console.CursorSize = 1;
            Console.CursorVisible = false;
            Console.SetWindowSize(240, 63);
        }

        static void ReStart()
        {
            ConsoleKeyInfo curKey;

            // 키 입력 확인
            curKey = GetKeyDown();

            if (curKey.Key == ConsoleKey.R)
            {
                GameStart();
            }

        }

        static void SaveData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("text.txt", FileMode.Create);

            SerializableDataField filesaver = new SerializableDataField();

            filesaver.highscore = highScore;

            bf.Serialize(fs, filesaver);
            fs.Close();
        }

        static void LoadDate()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("text.txt", FileMode.Open);

            SerializableDataField filesaver = new SerializableDataField();

            filesaver = bf.Deserialize(fs) as SerializableDataField;
            fs.Close();

            highScore = filesaver.highscore;
        }
    }

    [Serializable]
    class SerializableDataField
    {
        public int highscore;

    }

}
