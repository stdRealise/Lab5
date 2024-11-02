using System;

namespace Schedule
{
   class Schedule
   {
      private List<Trolleybus> trolleybuses;
      private string[] all_stations;
      int width;

      public Schedule(string file)
      {
         SortedSet<string> all_stations_set = new SortedSet<string>();
         trolleybuses = new List<Trolleybus>();
         FileStream fs = new FileStream(file, FileMode.Open);
         StreamReader sr = new StreamReader(fs);
         while (!sr.EndOfStream)
         {
            int number = int.Parse(sr.ReadLine());
            int interval = int.Parse(sr.ReadLine());
            List<string> stations = new List<string>();
            string[] time = sr.ReadLine().Split(' ', ':');
            int firstArrive = int.Parse(time[0]) * 60 + int.Parse(time[1]);
            int lastArrive = int.Parse(time[2]) * 60 + int.Parse(time[3]);
            time = sr.ReadLine().Split(' ', ':');
            int firstArriveR = int.Parse(time[0]) * 60 + int.Parse(time[1]);
            int lastArriveR = int.Parse(time[2]) * 60 + int.Parse(time[3]);
            string str = sr.ReadLine();
            if (all_stations_set.Add(str)) width = Math.Max(width, str.Length);
            stations.Add(str);
            List<int> times = new List<int>();
            while (!sr.EndOfStream)
            {
               str = sr.ReadLine();
               if (str == "")
               { 
                  break;
               }
               times.Add(int.Parse(str));
               str = sr.ReadLine();
               if (all_stations_set.Add(str)) width = Math.Max(width, str.Length);
               stations.Add(str);
            }
            trolleybuses.Add(new Trolleybus(number, interval, stations.ToArray(), times.ToArray(), firstArrive, lastArrive));
            stations.Reverse();
            times.Reverse();
            trolleybuses.Add(new Trolleybus(number, interval, stations.ToArray(), times.ToArray(), firstArriveR, lastArriveR));
            all_stations = all_stations_set.ToArray();
         }
         fs.Close();
      }

      public void PrintTimetable(int x, int y, string station, int width)
      {
         Console.ForegroundColor = ConsoleColor.DarkYellow;
         Console.SetCursorPosition(x, y);
         Console.Write(station);
         Console.SetCursorPosition(x + width - 5, y);
         Console.Write(DateTime.Now.ToString("t"));
         Console.ForegroundColor = ConsoleColor.White;
         Console.SetCursorPosition(x, y + 1);
         Console.Write("№  Направление");
         for (int i = 0; i < width - 28; i++)
         {
            Console.Write(' ');
         }
         Console.Write("Время прибытия");

         Console.ForegroundColor = ConsoleColor.DarkYellow;
         trolleybuses.Sort((tr1, tr2) => tr1.GetMinutesTo(station).CompareTo(tr2.GetMinutesTo(station)));
         for (int i = 0; i < Math.Min(trolleybuses.Count, 5); i++)
         {
            if(trolleybuses[i].GetMinutesTo(station) == 2000)
            {
               break;
            }
            Console.SetCursorPosition(x, y + i + 2);
            Console.Write("{0}  {1}", trolleybuses[i].Number, trolleybuses[i].GetDestination());
            Console.SetCursorPosition(x + width - 8, y + i + 2);
            Console.Write("{0} мин.", trolleybuses[i].GetMinutesTo(station));
         }
         Console.ForegroundColor = ConsoleColor.White;
      }

      public int FindStation(string station)
      {
         for(int i=0;i<all_stations.Length;i++)
         {
            if (all_stations[i] == station)
            {
               return i;
            }
         }
         return -1;
      }

      public int Width
      {
         get { return width; }
      }

      public List<string> GetStationsList()
      {
         return new List<string>(all_stations);
      }
   }
}
