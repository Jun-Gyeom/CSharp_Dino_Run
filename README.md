# CSharp_Dino_Run

# < 구글 크롬 공룡 게임>

## 게임 방법
- 다가오는 장애물을 피해 가능한 오래 살아남으세요.
- 버틴 시간 만큼 점수가 오릅니다.
- 선인장에 부딪히면 GameOver.
- ESC 키로 게임을 종료할 수 있습니다.

## 게임 기능
- 데이터 저장 기능이 있습니다.
- 게임을 완료하면 현재 점수와 최고 점수가 기록되는데, 이 때 게임을 종료하여도 데이터가 저장됩니다.

## 데이터 저장 코드
```c#
static void SaveData()
{
    BinaryFormatter bf = new BinaryFormatter();
    FileStream fs = new FileStream("text.txt", FileMode.Create);

    SerializableDataField filesaver = new SerializableDataField();

    filesaver.highscore = highScore;

    bf.Serialize(fs, filesaver);
    fs.Close();
}
```
## 데이터 불러오기 코드
        
```c#
static void LoadDate()
{
    BinaryFormatter bf = new BinaryFormatter();
    FileStream fs = new FileStream("text.txt", FileMode.Open);

    SerializableDataField filesaver = new SerializableDataField();

    filesaver = bf.Deserialize(fs) as SerializableDataField;
    fs.Close();

    highScore = filesaver.highscore;
}
```

### 콘솔 창에서 캐릭터와 장애물을 구현하기 위해 간단한 텍스트로 캐릭터와 장애물을 구현하였습니다.

## 공룡을 텍스트로 그리는 코드
```c#
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
```
#### 공룡의 상태에 따라 모션이 달라집니다.

## 장애물을 텍스트로 그리는 코드
```c#
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
```
#### 나무의 위치에 따라 모양이 달라집니다.
