using System;
using System.Timers;

namespace TimeAlarmConsoleApp
{
    // Define delegate for the alarm event
    public delegate void AlarmEventHandler();

    class AlarmClock
    {
        // Event declaration with default empty delegate to avoid null checks
        public event AlarmEventHandler RaiseAlarm = delegate { };

        private System.Timers.Timer timer;
        private TimeSpan targetTime;

        public AlarmClock()
        {
            timer = new System.Timers.Timer(1000); // Check every second
            timer.Elapsed += CheckAlarmTime;
        }

        public void SetAlarm(TimeSpan userTime)
        {
            targetTime = userTime;
            Console.WriteLine($"Alarm set for {targetTime:hh\\:mm\\:ss}. Waiting for alarm time...");
            Console.WriteLine("Press Ctrl+C to exit the application.");

            timer.Start();

            // This approach allows the timer to continue checking in the background
            Console.WriteLine("\nCurrent time: " + DateTime.Now.ToString("HH:mm:ss"));
            while (timer.Enabled)
            {
                System.Threading.Thread.Sleep(1000);
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("Current time: " + DateTime.Now.ToString("HH:mm:ss") + "     ");
            }
        }

        private void CheckAlarmTime(object sender, ElapsedEventArgs e)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (currentTime.Hours == targetTime.Hours &&
                currentTime.Minutes == targetTime.Minutes &&
                currentTime.Seconds == targetTime.Seconds)
            {
                timer.Stop();
                RaiseAlarm?.Invoke(); // Raise the event
            }
        }
    }

    class Program
    {
        static void RingAlarm()
        {
            Console.WriteLine("\nTarget time reached!!");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("==== Time Alarm Console Application ====");
            Console.Write("Enter alarm time (HH:MM:SS): ");
            string? input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input) && TimeSpan.TryParse(input, out TimeSpan alarmTime))
            {
                AlarmClock alarm = new AlarmClock();
                alarm.RaiseAlarm += RingAlarm; // Subscribe to the alarm event
                alarm.SetAlarm(alarmTime);
            }
            else
            {
                Console.WriteLine("Invalid time format. Please use HH:MM:SS format.");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
