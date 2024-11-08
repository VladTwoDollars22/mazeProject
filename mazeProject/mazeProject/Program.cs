//Програма має працювати завжди!

//Лабіринт
//Собака
//Керування
//Вихід
using System.Runtime.CompilerServices;

class Program
{
    static Random random = new Random();

    //field
    static char[,] field;
    static int width = 10;
    static int height = 12;
    static int block_freq = 28;

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
    static bool reached_finish = false;

    //JetPack
    static bool jetpackAvalaible = false;
    static bool jetpackIsActive = false;
    static int jetpackX = 0;
    static int jetpackY = 0;
    static int jetpackSpeed = 2;
    static int jetpackTimer = 0;

    static void generate_field()
    {
        field = new char[height, width];

        for (int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                char symbol;
                int rand_num = random.Next(0, 100);
                if (rand_num < block_freq)
                    symbol = '#';
                else
                    symbol = '.';
                field[i, j] = symbol;
            }
        }
    }

    static void draw()
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

    static void place_dog()
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

    static void generate()
    {
        generate_field();
        PlaceFinish();
        PlaceJetPack();
        place_dog();
    }

    static void get_input()
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
    static bool can_go_to(int newX, int newY)
    {
        if (newX < 0 || newY < 0 || newX >= width || newY >= height)
        {
            return false;
        }

        if (!is_walkable(newX, newY))
        {
            return false;
        }
        return true;
    }
    static void go_to(int newX,int newY)
    {
        dogX = newX;dogY = newY;
    }
    static void try_go_to(int newX,int newY)
    {
        if (can_go_to(newX, newY))
        {
            go_to(newX, newY);
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

    static void check_finish()
    {
        if (dogX == finishX && dogY == finishY)
        {
            reached_finish = true;
        }
    }

    static bool is_end_game()
    {
        return reached_finish;
    }

    static bool is_walkable(int X,int Y)
    {
        if (field[Y,X] == '#')
            return false;
        return true;
    }

    static void logic()
    {
        JetpackLogic();
        try_go_to(dogX + dx,dogY + dy);
        check_finish();
        CheckJetPack();
    }

    static void Main(string[] args)
    {
        generate();
        draw();

        while (!is_end_game())
        {
            get_input();
            logic();
            draw();
        }

        Console.WriteLine("Гравець переміг!!!");
    }
}

