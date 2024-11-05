//Програма має працювати завжди!

//Лабіринт
//Собака
//Керування
//Вихід
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
    static int dogX = 0;static int dogY = 0;

    //input
    static int dx = 0;static int dy = 0;

    //finish
    static int finishX = 0;static int finishY = 0;

    static bool reached_finish = false;
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
        finishX = random.Next(0, width - 1);
        finishY = random.Next(0, height - 1);
        field[finishY, finishX] = 'F';
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

    static void place_dog()
    {
        dogX = random.Next(0, width - 1);
        dogY = random.Next(0, height - 1);
    }

    static void generate()
    {
        generate_field();
        place_dog();
    }

    static (int dx, int dy) get_input()
    {
        dx = 0; dy = 0;
        string inp = Console.ReadLine();
        if (inp.Length == 0)
            return (0, 0);

        char first_symbol = inp[0];

        if (first_symbol == 'W' || first_symbol == 'w')
            dy = -1;
        if (first_symbol == 'A' || first_symbol == 'a')
            dx = -1;
        if (first_symbol == 'S' || first_symbol == 's')
            dy = 1;
        if (first_symbol == 'D' || first_symbol == 'd')
            dx = 1;

        return (dx, dy);
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

    static void check_finish()
    {
        if(dogX == finishX && dogY == finishY)
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
        try_go_to(dogX + dx,dogY + dy);

        check_finish();
    }

    static void Main(string[] args)
    {
        generate();
        draw();

        while (!is_end_game())
        {
            var input = get_input();dx = input.dx;dy = input.dy;
            logic();
            draw();
        }

        Console.WriteLine("Гравець переміг!!!");
    }
}

