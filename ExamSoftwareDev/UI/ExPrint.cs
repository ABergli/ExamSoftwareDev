using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSoftwareDesign.UI {
    public class ExPrint {

        public virtual void print(string txt, string c = "Gray") {
            switch (c)
            {
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.Write(txt);
        }

        public void log(int type, string txt, string c = "Gray") {
            // type: 0=normal print, 1= test, 2= warning?
            switch(type)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(txt);
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"test: {txt}");
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"warning: {txt}");
                    break;
            }
            
        }

    }
}
