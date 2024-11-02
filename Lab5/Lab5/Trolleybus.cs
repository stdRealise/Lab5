using System;


namespace Schedule
{
   class Trolleybus
   {
      int _number;
      int _interval;
      string[] _route;
      int[] _stationArrivals;
      int _lastArrival;
      public Trolleybus(int number, int interval, string[] route, int[] times, int firstArrival, int lastArrival)
      {
         _number = number;
         _interval = interval;
         _route = route;
         _stationArrivals = new int[_route.Length];
         _stationArrivals[0] = firstArrival;
         for(int i = 0; i < times.Length; i++)
         {
            _stationArrivals[i + 1] = _stationArrivals[i] + times[i];
         }
         _lastArrival = lastArrival;
      }

      private int FindStation(string station)
      {
         for (int i = 0; i < _route.Count(); i++)
         {
            if (_route[i] == station)
            {
               return i;
            } 
         }
         return -1;
      }
      public int GetMinutesTo(string station)
      {
         int idx = FindStation(station);
         if (idx == -1)
         {
            return 2000;
         }
         int now = DateTime.Now.Minute + DateTime.Now.Hour * 60;
         if (now < _stationArrivals[idx] || now > _lastArrival)
         {
            if (now > 720)
            {
               return 1440 - now + _stationArrivals[idx];
            }
            else
            {
               return _stationArrivals[idx] - now;
            }
         }
         else
         {
            int dif = (int)Math.Ceiling((decimal)(now - _stationArrivals[idx]) / _interval);
            return dif * _interval + _stationArrivals[idx] - now;
         }
      }

      public string GetDestination()
      {
         return _route[_route.Length - 1];
      }

      public int Number { get { return _number; } }
   }
}
