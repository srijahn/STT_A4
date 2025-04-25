using System;
using System.Drawing;
using System.Windows.Forms; 

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private TimeSpan alarmTime;
        private Random random;

        // Define delegate for the alarm event
        public delegate void AlarmEventHandler();

        // Event declaration with default empty delegate
        public event AlarmEventHandler RaiseAlarm = delegate { };

        public Form1()
        {
            InitializeComponent();

            // Set up the timer
            timer1.Interval = 1000; // 1 second

            // Subscribe to the alarm event
            RaiseAlarm += Ring_alarm;

            // Initialize random color generator
            random = new Random();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (TimeSpan.TryParse(textBoxTime.Text, out alarmTime))
            {
                timer1.Start();
                buttonStart.Enabled = false;
                textBoxTime.Enabled = false;
                this.Text = $"Alarm set for {alarmTime:hh\\:mm\\:ss}";
            }
            else
            {
                MessageBox.Show("Invalid time format. Please use HH:MM:SS format.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Change form background color
            this.BackColor = Color.FromArgb(
                random.Next(256), random.Next(256), random.Next(256));

            // Check if current time matches the alarm time
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (currentTime.Hours == alarmTime.Hours &&
                currentTime.Minutes == alarmTime.Minutes &&
                currentTime.Seconds == alarmTime.Seconds)
            {
                timer1.Stop();
                RaiseAlarm?.Invoke(); // Raise the alarm event
            }
        }

        private void Ring_alarm()
        {
            MessageBox.Show("Target time reached!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset form
            this.BackColor = SystemColors.Control;
            buttonStart.Enabled = true;
            textBoxTime.Enabled = true;
            this.Text = "Event-Driven Alarm App";
        }
    }
}