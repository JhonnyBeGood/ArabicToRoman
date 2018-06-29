using System;
using System.Collections.Generic;
using System.Text;

namespace ArabicConvert
{
    class ValidationCheck
    {
        internal bool Checking(StringBuilder builder)
        {
            int rezult;
            bool isCorrect = Int32.TryParse(builder.ToString(),  out rezult);
            return isCorrect;
        }
    }

    class ArabicToRoman
    {

    }

    class Program
    {
        private static void RomanNumbers()
        {
            _orderRoman.Add(1, "I");
            _orderRoman.Add(4, "IV");
            _orderRoman.Add(5, "V");
            _orderRoman.Add(9, "IX");
            _orderRoman.Add(10, "X");
            _orderRoman.Add(40, "XL");
            _orderRoman.Add(50, "L");
            _orderRoman.Add(90, "XC");
            _orderRoman.Add(100, "C");
            _orderRoman.Add(400, "CD");
            _orderRoman.Add(500, "D");
            _orderRoman.Add(900, "CM");
            _orderRoman.Add(1000, "M");
            _orderRoman.Add(4000, "MỲ");
            _orderRoman.Add(5000, "Ỳ");
            _orderRoman.Add(9000, "ỲŴ");
            _orderRoman.Add(10000, "Ŵ");
        }
        //хранилище введенного значения в консоль
        readonly StringBuilder _builder = new StringBuilder();
        //проверка того, что введена цифра
        readonly ValidationCheck _check = new ValidationCheck();
        //словарь в котором храним заданное число в формате отдельно единицы, десятки, сотни, тысячи и т.д.
        private static readonly Dictionary<int, int> _orderNumbers = new Dictionary<int, int>();
        //словарь, в котором храним базовые занчения "Римских цифр"
        private static readonly Dictionary<int, string> _orderRoman = new Dictionary<int,string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello! This is convertor from Arabic numerals to Romon numerals.");
            Console.WriteLine("If you want to stop appication press Escape");
            //Заполняем словарь базовами, эталонными значениями
            RomanNumbers();

            do
            {
                //вызываем сам метод преобразования
               new Program().Start();
                _orderNumbers.Clear();

            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        
        public void Start()
        {
            Console.WriteLine("");
            Console.WriteLine("Please enter a value:");
            Console.WriteLine("");
            _builder.Append(Console.ReadLine());
            if (!_check.Checking(_builder))
            {
                Console.WriteLine("Invalid characters used, please retype");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine(ConverToRoman(_builder));
            }
        }

        /// <summary>
        /// метод в тором происходит само преобразование
        /// </summary>
        private string ConverToRoman(StringBuilder builder)
        {
            //список ключей словаря "Римские фицры"
            List<int> keysList = new List<int>(_orderRoman.Keys);
            //переменная - результат перевода арабских в римские
            var rezult = "";
            //проверяем, вдруг пользователь ввёл не "56", а "00056"
            if (Convert.ToInt32(builder.ToString()).ToString().Length != builder.Length)
            {
                string oldString = builder.ToString();
                string newString = Convert.ToInt32(builder.ToString()).ToString();
                builder.Replace(oldString, newString);
            }
            //если пользователь ввёл значение, которое сразу есть в словаре, то выдаём результат
            if (keysList.Exists(x1 => x1 == Convert.ToInt16(builder.ToString())))
                return rezult = _orderRoman[Convert.ToInt16(builder.ToString())];

            //цикл, в которм разделяем исходное значение на составляющие (десятки, сотни, тысячи)
            for (int i = 0; i < builder.Length; i++)
            {
                int number = Convert.ToInt16(builder.ToString().ToCharArray()[i].ToString());
                if (builder.Length == 1)
                    _orderNumbers.Add(1, number);
                else
                    _orderNumbers.Add((int) Math.Pow(10, (builder.Length - 1) - i), number);
            }

            //цикл, в котором каждое значение переводим в эквивалент римского
            foreach (KeyValuePair<int, int> valuePair in _orderNumbers)
            {
                //получаем ближайшее значение к нашему из списка ключей "римских" 
                int index = keysList.FindIndex(x1 => ((valuePair.Key * valuePair.Value) - x1 <= 0));
                //получаем значение сотвествующее этому ключу
                int value = keysList.Find(x1 => ((valuePair.Key * valuePair.Value) - x1 <= 0));

                //проверяем значение меньше нашего
                if (value > (valuePair.Key * valuePair.Value))
                {
                    //в Римском нет значение "0"
                    if (index<=0)
                    {
                        continue;
                    }
                    index = ((index - 1) < 0) ? (index = 0) : (index - 1);
                    rezult = rezult + _orderRoman[keysList[index]];
                }
                else
                //если попали сюда, значит цифра соответствует значению в словаре "римских"
                    rezult = rezult + _orderRoman[keysList[index]];

                //вычисляем сколько раз надо добавить. Например 300, значит надо ещё два раза добавить римскую букву
                int cycleValue = ((valuePair.Key * valuePair.Value) - keysList[index]) / valuePair.Key;

                for (int i = 0; i < cycleValue; i++)
                {
                    if (valuePair.Key == 1)
                        rezult = rezult + _orderRoman[keysList[0]];
                    else
                        rezult = rezult + _orderRoman[keysList.Find(x1 => (x1 == valuePair.Key))];
                }
            }
            return rezult;
        }
    }
}
