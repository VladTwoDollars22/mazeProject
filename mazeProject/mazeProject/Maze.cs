class Program
{
    static void Main(string[] args)
    {
        Maze maze = new Maze();
        maze.GameProcess();
    }
}
public class Maze
{
    static Random random = new Random();

    //field
    static char[,] field;
    static int width = 10;
    static int height = 12;
    static int blockFreq = 28;

    //dog
    static char dog = '@';
    static int dogX = 0;
    static int dogY = 0;
    static int dogSpeed = 1;

    //input
    static int dx = 0;static int dy = 0;

    //finish
    static int finishX = 0;
    static int finishY = 0;
    static bool reachedFinish = false;

    //JetPack
    static bool jetpackAvalaible = false;
    static bool jetpackIsActive = false;
    static int jetpackX = 0;
    static int jetpackY = 0;
    static int jetpackSpeed = 2;
    static int jetpackTimer = 0;

    //GameTimer
    static int gameTime = 15;

    //GameEnd
    static string gameEndSubtitle;

    static void GenerateField()
    {
        field = new char[height, width];

        for (int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                char symbol;
                int rand_num = random.Next(0, 100);
                if (rand_num < blockFreq)
                    symbol = '#';
                else
                    symbol = '.';
                field[i, j] = symbol;
            }
        }
    }

    static void Draw()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                char symbol;
                if (i == dogY && j == dogX)
                    symbol = dog;
                else
                    symbol = field[i, j];
                Console.Write(symbol);
            }

            Console.WriteLine();
        }
    }

    static void PlaceJetPack()
    {
        var point = GetRandomPoint();

        jetpackX = point.X;
        jetpackY = point.Y;

        field[jetpackY, jetpackX] = 'J';
    }
    static void PlaceFinish()
    {
        var point = GetRandomPoint();

        finishX = point.X;
        finishY = point.Y;

        field[finishY, finishX] = 'F';
    }

    static void PlaceDog()
    {
        var point = GetRandomPoint();

        dogX = point.X;
        dogY = point.Y;
    }

    static (int X,int Y) GetRandomPoint()
    {
        int pointX = random.Next(0, width - 1);
        int pointY = random.Next(0, height- 1);

        return (pointX, pointY);
    }

    static void Generate()
    {
        GenerateField();
        PlaceFinish();
        PlaceJetPack();
        PlaceDog();
    }

    static void GetInput()
    {
        dx = 0; dy = 0;
        string inp = Console.ReadLine();

        if (inp.Length == 0)
            return;

        char first_symbol = inp[0];

        CheckActionsInput(first_symbol);
        CheckMovementInput(first_symbol);
    }

    static void CheckActionsInput(char symbol)
    {
        if (symbol == 'J' || symbol == 'j' && jetpackAvalaible)
        {
            jetpackIsActive = true;
            jetpackTimer = 3;
            jetpackAvalaible = false;
            return;
        }
    }

    static void CheckMovementInput(char symbol)
    {
        switch (symbol)
        {
            case 'W' or 'w':
                dy = -dogSpeed;
                break;

            case 'A' or 'a':
                dx = -dogSpeed;
                break;

            case 'S' or 's':
                dy = dogSpeed;
                break;

            case 'D' or 'd':
                dx = dogSpeed;
                break;

        }
    }

    static void JetpackLogic()
    {
        jetpackTimer--;

        if (jetpackTimer == 0)
        {
            jetpackIsActive = false;
            return;
        }

        if (jetpackIsActive == true)
        {
            dx *= jetpackSpeed;
            dy *= jetpackSpeed;
        }
    }
    static bool CanGoTo(int newX, int newY)
    {
        if (newX < 0 || newY < 0 || newX >= width || newY >= height)
        {
            return false;
        }

        if (!IsWalkable(newX, newY))
        {
            return false;
        }

        return true;
    }
    static void GoTo(int newX,int newY)
    {
        dogX = newX;
        dogY = newY;
    }
    static void TryGoTo(int newX,int newY)
    {
        if (CanGoTo(newX, newY))
        {
            GoTo(newX, newY);
        }
    }

    static void CheckJetPack()
    {
        if(dogX == jetpackX && dogY == jetpackY)
        {
            jetpackAvalaible = true;

            field[jetpackY, jetpackX] = '.';

            PlaceJetPack();
        }
    }

    static void CheckFinish()
    {
        if (dogX == finishX && dogY == finishY)
        {
            reachedFinish = true;
        }
    }

    static bool IsEndGame()
    {
        if(gameTime == 0)
        {
            gameEndSubtitle = "Час скінчився...Стіни зімкнулись й собака перетворилась в начинку для хотдогу";
            return true;
        }

        gameEndSubtitle = "Гравець дійшов до фініша та переміг!";
        return reachedFinish;
    }

    static bool IsWalkable(int X,int Y)
    {
        if (field[Y,X] == '#')
            return false;

        return true;
    }

    static void TimerLogic()
    {
        gameTime--;

        Console.WriteLine("Лишилось ходів:" + gameTime);
    }

    static void Logic()
    {
        JetpackLogic();
        TryGoTo(dogX + dx,dogY + dy);
        CheckFinish();
        CheckJetPack();
        TimerLogic();
    }
    public void GameProcess()
    {
        Generate();
        Draw();

        while (!IsEndGame())
        {
            GetInput();
            Logic();
            Draw();
        }

        Console.WriteLine(gameEndSubtitle);
    }
}

