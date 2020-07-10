using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triple;

namespace tripleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            TripleGame game = new TripleGame();
            game.Start(true, 7, 7, 3);
            while (true)
            {
                System.Console.WriteLine("enter act: 'rowIndex columnIndex act' (act: up=0, down=1, left=2, right=3)");
                string s = Console.ReadLine();
                if (s == "Q")
                {
                    break;
                }
                string[] acts = s.Split();
                game.DoAction(Convert.ToInt32(acts[0]), Convert.ToInt32(acts[1]), Convert.ToInt32(acts[2]));
            }
        }
    }
}
