using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace StringBot.Controllers
{
    public class Calculator
    {
        public int Sum(string message)
        {
            int summ = 0;

            for (int i = 0; i < message.Length; i++)
            {
                if (char.IsNumber(message[i]))
                {
                    summ += Convert.ToInt32(message[i].ToString());
                }
            }

            return summ;
        }

    }
}
