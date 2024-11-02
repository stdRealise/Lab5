using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Schedule
{
   class Program
   {
      static void PrintTitle(int x, int y)
      {
         Console.BackgroundColor = ConsoleColor.Cyan;
         Console.ForegroundColor = ConsoleColor.Black;
         Console.SetCursorPosition(x, y);
         Console.Write(" Расписание Маршрутов Троллейбусов ");
      }

      static void PrintStartScreen(int x, int y)
      {
         PrintTitle(x, y);
         Console.BackgroundColor = ConsoleColor.Black;
         Console.ForegroundColor = ConsoleColor.Gray;
         Console.SetCursorPosition(x + 2, y + 2);
         Console.Write("[Для продолжения нажмите Enter]");
         Console.ForegroundColor = ConsoleColor.White;
         Console.ReadLine();
         Console.Clear();
      }

      static void PrintBox(int x, int y, string name, int height, int width = 0, ConsoleColor color = ConsoleColor.Cyan)
      {
         int width_without_name = width;
         if (width < name.Length)
         {
            width_without_name = 10;
         }
         else
         {
            width_without_name -= name.Length;
         }
         Console.SetCursorPosition(x - 1, y - 1);
         Console.BackgroundColor = ConsoleColor.Black;
         Console.ForegroundColor = color;
         Console.Write("╔");
         for (int i = 0; i < width_without_name / 2; i++)
         {
            Console.Write("═");
         }
         Console.BackgroundColor = color;
         Console.ForegroundColor = ConsoleColor.Black;
         Console.Write(name);
         Console.BackgroundColor = ConsoleColor.Black;
         Console.ForegroundColor = color;
         for (int i = 0; i < width_without_name / 2 + width_without_name % 2; i++)
         {
            Console.Write('═');
         }
         Console.Write('╗');
         for (int i = 0; i < height; i++)
         {
            Console.SetCursorPosition(x - 1, y + i);
            Console.Write('║');
            Console.SetCursorPosition(x + name.Length + width_without_name, y + i);
            Console.Write('║');
         }
         Console.SetCursorPosition(x - 1, y + height);
         Console.Write('╚');
         for (int i = 0; i < name.Length + width_without_name; i++)
         {
            Console.Write('═');
         }
         Console.Write('╝');
         Console.ForegroundColor = ConsoleColor.White;
      }

      static void PrintTimetable(int x, int y, string station, Schedule schedule, int width)
      {
         PrintTitle(x + 22, y);
         Console.BackgroundColor = ConsoleColor.Black;
         Console.ForegroundColor = ConsoleColor.Gray;
         Console.SetCursorPosition(x + 24, y + 2);
         Console.Write("[Для продолжения нажмите Enter]");
         PrintBox(x + 22, y + 5, " Расписание ", 7, width);
         schedule.PrintTimetable(x + 22, y + 5, station, width);
         Console.ReadLine();
         Console.Clear();
      }

      static void PrintStations(int x, int y, int active_i, int active_j, List<string> stations, int width)
      {
         PrintTitle(x + 22, y);
         Console.SetCursorPosition(x + 14, y + 2);
         Console.BackgroundColor = ConsoleColor.Black;
         Console.ForegroundColor = ConsoleColor.Gray;
         Console.Write("[Введите название остановки или выберите из списка]");
         PrintBox(x, y + 5, " Остановки ", 20, width * 3);
         PrintBox(x, y + 27, " Введите название остановки: ", 1, width * 3);
         int i = 0;
         int j = 0;
         foreach (string station in stations)
         {
            Console.SetCursorPosition(x + j, y + i + 5);
            Console.Write(station);
            i++;
            if (i == 20)
            {
               i = 0;
               j += width;
            }
         }
         Console.BackgroundColor = ConsoleColor.Yellow;
         Console.ForegroundColor = ConsoleColor.Black;
         Console.SetCursorPosition(x + active_j * width, y + active_i + 5);
         Console.Write(stations[active_i + active_j * 20]);
         Console.BackgroundColor = ConsoleColor.Black;
         Console.ForegroundColor = ConsoleColor.White;
      }

      static void SelectMenuItem(int x, int y, Schedule schedule)
      {
         bool isWorking = true;
         int active_i = 0;
         int active_j = 0;
         List<string> stations = schedule.GetStationsList();
         while (isWorking)
         {
            PrintStations(x, y, active_i, active_j, stations, schedule.Width + 1);
            ConsoleKeyInfo info = Console.ReadKey();
            int n = stations.Count() / 20;
            switch (info.Key)
            {
               case ConsoleKey.Enter:
                  Console.Clear();
                  PrintTimetable(x, y, stations[active_i + active_j * 20], schedule, 35);
                  break;
               case ConsoleKey.UpArrow:
                  if (active_i > 0)
                  {
                     active_i--;
                  }
                  else
                  {
                     if (active_j == n || n == 0) active_i = stations.Count() % 20 - 1;
                     else active_i = 19;
                  }
                  break;
               case ConsoleKey.DownArrow:
                  if (active_j == n || n == 0) n = stations.Count() % 20;
                  else n = 20;
                  if (active_i == n - 1)
                  {
                     Console.SetCursorPosition(x, y + 27);
                     Console.CursorVisible = true;
                     string name = Console.ReadLine();
                     if(name == "")
                     {
                        isWorking = false;
                     }
                     else
                     {
                        Console.CursorVisible = false;
                        if (schedule.FindStation(name) == -1)
                        {
                           Console.SetCursorPosition(x, y + 27);
                           Console.ForegroundColor = ConsoleColor.Red;
                           Console.Write("Остановка не найдена! Для продолжения нажмите Enter.");
                           Console.ForegroundColor = ConsoleColor.White;
                           Console.ReadLine();
                           Console.Clear();
                        }
                        else
                        {
                           Console.Clear();
                           PrintTimetable(x, y, name, schedule, 35);
                        }
                        active_i = n - 1;
                     }
                  }
                  else
                  {
                     active_i++;
                  }
                  break;
               case ConsoleKey.RightArrow:
                  if ( active_j != n - 1 || active_i < stations.Count() % 20) n++;
                  if (active_j < n - 1)
                  {
                     active_j++;
                  }
                  else
                  {
                     active_j = 0;
                  }
                  break;
               case ConsoleKey.LeftArrow:
                  if (active_j != 0 || active_i < stations.Count() % 20) n++;
                  if (active_j > 0)
                  {
                     active_j--;
                  }
                  else
                  {
                     active_j = n - 1;
                  }
                  break;
            }
         }
      }

      static void PrintAll(int x, int y, Schedule schedule)
      {
         PrintStartScreen(x + 22, y);
         SelectMenuItem(x, y, schedule);
      }

      static void Main(string[] args)
      {
         Console.CursorVisible = false;
         Schedule schedule = new Schedule("Schedule.txt");
         PrintAll(25, 1, schedule);
         Console.CursorVisible = true;
      }
   }
}