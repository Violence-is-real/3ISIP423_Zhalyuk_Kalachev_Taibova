using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr7
{
    internal class Program
    {
        static string readString()
        {
            string str = "";
            while (str == "")
            {
                str = Console.ReadLine();
                if (str == "")
                {
                    Console.WriteLine("Некорректные данные");
                }
            }
            return str;
        }

        static string readYN()
        {
            string str = "";
            while (!(str.ToLower() == "y" || str.ToLower() == "n"))
            {
                str = Console.ReadLine();
                if (!(str.ToLower() == "y" || str.ToLower() == "n"))
                {
                    Console.WriteLine("Некорректные данные");
                }
            }
            return str.ToLower();
        }

        static int readInt()
        {
            int q = 0;
            bool flag = true;
            while (flag)
            {
                flag = false;
                if (!(int.TryParse(Console.ReadLine(), out q)))
                {
                    Console.WriteLine("Некорректные данные");
                    flag = true;
                }
                if (q <= 0)
                {
                    Console.WriteLine("Некорректные данные");
                    flag = true;
                }
            }
            return q;
        }
        static int readIntLimit(int lim)
        {
            int q = 0;
            bool flag = true;
            while (flag)
            {
                flag = false;
                if (!(int.TryParse(Console.ReadLine(), out q)))
                {
                    Console.WriteLine("Некорректные данные");
                    flag = true;
                }
                if (q <= 0 || q > lim)
                {
                    Console.WriteLine("Некорректные данные");
                    flag = true;
                }
            }
            return q;
        }

        static void Stock(ref List<SparePart> parts)
        {
            Console.Clear();
            Console.WriteLine("Склад:");
            foreach (var i in parts)
            {
                if (i.Stock > 0) Console.WriteLine($"{i.Name} x{i.Stock}");
            }
            Console.WriteLine();
            Console.WriteLine("1. Назад");
            readIntLimit(1);
        }

        static void Shop(ref List<SparePart> parts, ref AutoService editService, ref SparePart purchasedPart, ref int purchaseCounter)
        {
            Console.Clear();
            Console.WriteLine($"Деньги: {editService.Money}\n");
            Console.WriteLine("Товары:");
            foreach (var i in parts)
            {
                Console.WriteLine($"{i.SparePart_ID}. {i.Name} | Цена: {i.Price}");
            }
            Console.WriteLine();
            Console.WriteLine("Будете покупать? (Y/N)");
            string ch = readYN();
            if (ch == "y")
            {
                Console.WriteLine("Какой товар хотите купить? (Номер)");
                int choice = readIntLimit(parts.Count);
                if (editService.Money >= parts[choice - 1].Price)
                {
                    purchasedPart = parts[choice - 1]; // Запоминаем купленную деталь
                    purchaseCounter = 0; // Сбрасываем счетчик
                    Console.WriteLine($"Деталь будет доставлена после обслуживания {2 - purchaseCounter} клиентов!");

                    editService.Money -= parts[choice - 1].Price;
                    Core.Context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Hm... Come back when you're a little... Hm... Richer!");
                }
            }
        }

        static void Repair(ref List<SparePart> parts, ref AutoService editService, ref Client cur_client, ref SparePart purchasedPart, ref int purchaseCounter)
        {
            Console.Clear();
            if (parts[cur_client.SparePart_ID].Stock == 0)
            {
                List<int> prts = new List<int>();
                foreach (var part in parts) if (part.Stock > 0) prts.Add(part.SparePart_ID);
                Random rnd = new Random();
                int rand = prts[rnd.Next(prts.Count)];

                parts[rand].Stock -= 1;
                Core.Context.SaveChanges();

                Console.WriteLine($"Вы заменили клиенту не ту деталь и получили штраф {5000} рублей!");
                Console.WriteLine($"Клиент оставил плохой отзыв.");
                Console.WriteLine($"Вы потеряли {parts[rand].Name} x1");

                cur_client.Result = 2;
                Core.Context.Client.Add(cur_client);
                Core.Context.SaveChanges();

                editService.Money -= 5000;
                Core.Context.SaveChanges();
            }
            else
            {
                parts[cur_client.SparePart_ID].Stock -= 1;
                Core.Context.SaveChanges();

                Console.WriteLine($"Вы обслужили клиента и получили {parts[cur_client.SparePart_ID].Price * 3 / 2} рублей!");
                Console.WriteLine($"Клиент оставил хороший отзыв.");

                cur_client.Result = 1;
                Core.Context.Client.Add(cur_client);
                Core.Context.SaveChanges();

                editService.Money += parts[cur_client.SparePart_ID].Price * 5 / 4;
                Core.Context.SaveChanges();
            }

            // Логика доставки купленной детали
            if (purchasedPart != null)
            {
                purchaseCounter++;
                if (purchaseCounter >= 2)
                {
                    purchasedPart.Stock += 1;
                    Core.Context.SaveChanges();
                    Console.WriteLine($"\nДоставлена купленная ранее деталь: {purchasedPart.Name}!");
                    purchasedPart = null; // Сбрасываем
                    purchaseCounter = 0; // Обнуляем счетчик
                }
            }

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            List<SparePart> parts = Core.Context.SparePart.ToList();
            List<Client> clients = Core.Context.Client.ToList();

            Random rnd = new Random();

            // reset

            foreach (var i in parts) i.Stock = 0;

            foreach (var i in clients) Core.Context.Client.Remove(i);

            SparePart editPart = Core.Context.SparePart.First(u => u.Name.Contains("Шины Летние"));
            editPart.Stock = 2;
            Core.Context.SaveChanges();

            editPart = Core.Context.SparePart.First(u => u.Name.Contains("Шины Зимние"));
            editPart.Stock = 1;
            Core.Context.SaveChanges();

            editPart = Core.Context.SparePart.First(u => u.Name.Contains("Фильтры"));
            editPart.Stock = 1;
            Core.Context.SaveChanges();

            AutoService editService = Core.Context.AutoService.First(u => u.ID < 2);
            editService.Money = 20000;
            Core.Context.SaveChanges();

            List<String> ClientNameList = new List<String> { "Dan", "Polly", "Jack", "April", "Liam", "Max", "Mike",
            "Frank", "Sam", "Kate", "Andrew"};

            // reset stop

            int purchaseCounter = 0; // Счетчик клиентов после покупки
            SparePart purchasedPart = null; // Купленная деталь

            Client cur_client = new Client();

            int comm;
            bool ClientHere = false;

            Console.WriteLine($"Вы - владелец автосервиса.\n\nВы только открыли свой бизнес и сейчас у вас есть лишь несколько деталей и {editService.Money} рублей." +
                $"\nК вам будут ехать клиенты. Ваша цель - починить их машины деталями на вашем складе, которые можно купить в магазине.");

            Console.ReadLine();
            Console.Clear();

            while (editService.Money != 0)
            {
                if (!ClientHere)
                {
                    cur_client = new Client
                    {
                        Client_ID = clients.Count + 1,
                        Name = ClientNameList[rnd.Next(ClientNameList.Count)],
                        SparePart_ID = rnd.Next(parts.Count),
                        Result = 0
                    };
                    clients.Add(cur_client);

                    Console.WriteLine($"У вас новый клиент - {cur_client.Name}\nУ машины требуется починка - {parts[cur_client.SparePart_ID].Name}");

                    ClientHere = true;
                    Console.ReadLine();
                }
                else if (ClientHere)
                {
                    Console.WriteLine($"Деньги: {editService.Money}\n");
                    Console.WriteLine($"Клиент - {cur_client.Name}\nУ машины требуется починка - {parts[cur_client.SparePart_ID].Name}\n");

                    Console.WriteLine($"Действия:");
                    Console.WriteLine($"1. Посмотреть склад");
                    Console.WriteLine($"2. В магазин");
                    Console.WriteLine($"3. Отказать в обслуживании");
                    Console.WriteLine($"4. Обслужить");

                    comm = readIntLimit(4);

                    switch (comm)
                    {
                        case 1:
                            Stock(ref parts);
                            break;
                        case 2:
                            Shop(ref parts, ref editService, ref purchasedPart, ref purchaseCounter);
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine($"За отказ с вас взяли штраф {2000} рублей!");
                            editService.Money -= 2000;
                            Core.Context.SaveChanges();

                            // Увеличиваем счетчик и для отказа
                            if (purchasedPart != null)
                            {
                                purchaseCounter++;
                                if (purchaseCounter >= 2)
                                {
                                    purchasedPart.Stock += 1;
                                    Core.Context.SaveChanges();
                                    Console.WriteLine($"\nДоставлена купленная ранее деталь: {purchasedPart.Name}!");
                                    purchasedPart = null;
                                    purchaseCounter = 0;
                                }
                            }

                            ClientHere = false;
                            Console.ReadLine();
                            break;
                        case 4:
                            Repair(ref parts, ref editService, ref cur_client, ref purchasedPart, ref purchaseCounter);
                            ClientHere = false;
                            break;
                    }
                }
                Console.Clear();
            }
            Console.WriteLine($"Вы обанкротились!");
        }
    }
}