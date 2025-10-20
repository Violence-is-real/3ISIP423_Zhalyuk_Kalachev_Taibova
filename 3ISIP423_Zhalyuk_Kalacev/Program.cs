using System;
using System.Collections.Generic;

namespace TextRoguelike
{
    public class Item
    {
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public Item(string name, int attack = 0, int defense = 0)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }
    }

    public class Weapon : Item
    {
        public Weapon(string name, int attack) : base(name, attack, 0) { }
    }

    public class Armor : Item
    {
        public Armor(string name, int defense) : base(name, 0, defense) { }
    }

    public class Enemy
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public double CritChance { get; set; }
        public double FreezeChance { get; set; }
        public bool IgnoreDefense { get; set; }

        public Enemy(string name, int hp, int attack, int defense,
                    double critChance = 0, double freezeChance = 0, bool ignoreDefense = false)
        {
            Name = name;
            MaxHP = hp;
            HP = hp;
            Attack = attack;
            Defense = defense;
            CritChance = critChance;
            FreezeChance = freezeChance;
            IgnoreDefense = ignoreDefense;
        }

        public bool IsAlive() => HP > 0;
    }

    public class Player
    {
        public int MaxHP { get; set; } = 100;
        public int HP { get; set; } = 100;
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public bool IsFrozen { get; set; }

        public Player()
        {
            Weapon = new Weapon("Кулаки", 5);
            Armor = new Armor("Кожаные куртки", 2);
        }

        public bool IsAlive() => HP > 0;

        public int GetAttack() => Weapon.Attack;
        public int GetDefense() => Armor.Defense;

        public void TakeDamage(int damage)
        {
            HP = Math.Max(0, HP - damage);
        }

        public void Heal()
        {
            HP = MaxHP;
        }
    }

    class Program
    {
        private static Player player;
        private static Random random;
        private static int turnCount;

        private static List<Enemy> enemies;
        private static List<Enemy> bosses;
        private static List<Weapon> weapons;
        private static List<Armor> armors;

        static void Main(string[] args)
        {
            InitializeGame();
            StartGame();
        }

        static void InitializeGame()
        {
            player = new Player();
            random = new Random();
            turnCount = 0;

            // Создаем обычных врагов
            enemies = new List<Enemy>
            {
                new Enemy("Гоблин", 30, 8, 3, critChance: 0.2),
                new Enemy("Скелет", 25, 10, 2, ignoreDefense: true),
                new Enemy("Маг", 20, 12, 1, freezeChance: 0.15)
            };

            // Создаем боссов
            bosses = new List<Enemy>
            {
                new Enemy("ВВГ", 60, 12, 4, critChance: 0.3),
                new Enemy("Ковальский", 63, 13, 3, ignoreDefense: true),
                new Enemy("Архимаг C++", 36, 19, 1, freezeChance: 0.25),
                new Enemy("Пестов С--", 33, 18, 1, freezeChance: 0.3, ignoreDefense: true)
            };

            // Создаем оружие
            weapons = new List<Weapon>
            {
                new Weapon("Ржавый меч", 10),
                new Weapon("Острый кинжал", 15),
                new Weapon("Боевой топор", 20),
                new Weapon("Легендарный клинок", 30)
            };

            // Создаем броню
            armors = new List<Armor>
            {
                new Armor("Кожаная броня", 5),
                new Armor("Кольчуга", 10),
                new Armor("Латные доспехи", 15),
                new Armor("Драконья чешуя", 25)
            };
        }

        static void StartGame()
        {
            Console.WriteLine("Добро пожаловать в текстовый рогалик!");
            Console.WriteLine("Управление: вводите цифры для выбора действий");

            while (player.IsAlive())
            {
                ProcessTurn();
            }

            Console.WriteLine("\nИгра окончена. Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void ProcessTurn()
        {
            turnCount++;
            Console.WriteLine($"\n=== Ход {turnCount} ===");

            if (player.IsFrozen)
            {
                Console.WriteLine("Вы заморожены и пропускаете ход!");
                player.IsFrozen = false;
                return;
            }

            if (turnCount % 10 == 0)
            {
                BossFight();
            }
            else
            {
                if (random.NextDouble() < 0.5)
                {
                    EnemyEncounter();
                }
                else
                {
                    ChestEncounter();
                }
            }
        }

        static void EnemyEncounter()
        {
            var enemy = enemies[random.Next(enemies.Count)];
            Console.WriteLine($"На вас напал {enemy.Name}!");
            Combat(enemy);
        }

        static void BossFight()
        {
            var boss = bosses[random.Next(bosses.Count)];
            Console.WriteLine($"!!! ПОЯВИЛСЯ БОСС {boss.Name} !!!");
            Combat(boss);
        }

        static void Combat(Enemy enemy)
        {
            while (enemy.IsAlive() && player.IsAlive())
            {
                // Ход игрока
                Console.WriteLine($"\nВаше HP: {player.HP}/{player.MaxHP}");
                Console.WriteLine($"{enemy.Name}: HP {enemy.HP}/{enemy.MaxHP}");
                Console.Write("Выберите действие: [1] Атака [2] Защита\n> ");

                var choice = Console.ReadLine();
                double blockPower = 0;

                if (choice == "1")
                {
                    var damage = player.GetAttack();
                    enemy.HP -= damage;
                    Console.WriteLine($"Вы нанесли {damage} урона!");
                }
                else if (choice == "2")
                {
                    if (random.NextDouble() < 0.4)
                    {
                        Console.WriteLine("Вы уклонились от атаки!");
                        continue;
                    }
                    else
                    {
                        blockPower = random.NextDouble() * 0.3 + 0.7; // 70-100%
                        blockPower *= player.GetDefense();
                        Console.WriteLine($"Вы приготовились блокировать урон (сила блока: {blockPower:F1})");
                    }
                }

                // Ход врага
                if (enemy.IsAlive())
                {
                    // Проверка заморозки
                    if (random.NextDouble() < enemy.FreezeChance)
                    {
                        Console.WriteLine($"{enemy.Name} замораживает вас! Вы пропустите следующий ход.");
                        player.IsFrozen = true;
                    }

                    var damage = enemy.Attack;

                    // Проверка крита
                    if (random.NextDouble() < enemy.CritChance)
                    {
                        damage *= 2;
                        Console.WriteLine("Критический урон!");
                    }

                    // Игнор защиты
                    int finalDamage;
                    if (enemy.IgnoreDefense)
                    {
                        finalDamage = damage;
                    }
                    else
                    {
                        var defense = choice == "2" ? (int)blockPower : player.GetDefense();
                        finalDamage = Math.Max(1, damage - defense);
                    }

                    player.TakeDamage(finalDamage);
                    Console.WriteLine($"{enemy.Name} наносит вам {finalDamage} урона!");
                }
            }

            if (player.IsAlive())
            {
                Console.WriteLine($"\nВы победили {enemy.Name}!");
            }
            else
            {
                Console.WriteLine("\nВы погибли...");
            }
        }

       
        }
    }
}