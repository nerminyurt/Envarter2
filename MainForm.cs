using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Envarter2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        int ticks;
        Thread thread1, thread2;
        private async void btn_start_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Start();

                await Event_Producer();
                 //thread1 = new Thread(new ThreadStart(async () =>  await Event_Producer()));
                // thread1.Start();
                thread2 = new Thread(new ThreadStart(async () => await Event_Consumer()));
                //await Event_Consumer();
                thread2.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
        }

            public static EventLevel GenerateRandomEvent()
        {
            EventLevel evnt = null;
            try { 
            Array values = Enum.GetValues(typeof(Level));
             evnt = new EventLevel();
            Random rand = new Random();
            evnt.Pripority = (Level)values.GetValue(rand.Next(values.Length));
            Console.WriteLine(evnt.Pripority);
        }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return evnt;
        }

        public static  async  Task Event_Producer() 
        {
            try
            {
               
                    for (int i = 0; i <400; i++)
                    {
                    ThreadPool.QueueUserWorkItem((obj) =>
                    {
                        EventLevel e1 = GenerateRandomEvent();

                        PutEvent(e1);
                    }, i);
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static  SimpleStack<EventLevel> stack = new SimpleStack<EventLevel>();

        public static void PutEvent(EventLevel evnt)
        {
           
                stack.Push(evnt);
                Thread.Sleep(3000);
            
        }

        public async Task Event_Consumer()
        {
            try {
              
               
            for (int i = 2; i < 400; i++)
            {
                    EventLevel ev1 = ReadEvent(0);
                    EventLevel ev2 = ReadEvent(1);
                   

                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    EventLevel ev3 = ReadEvent(i);
                    if (ev1.Pripority == ev2.Pripority && ev2.Pripority == ev3.Pripority)
                    {
                        MessageBox.Show(ev1 + "-" + ev2 + "-" + ev3 + "--->" + ev1.Pripority);
                        ev1 = ev2;
                        ev2 = ev3;
                        
                    }
                    Invoke((Action)(() =>
                    {
                        prgbar.Value++;
                       
                        lbl_result.Text = "%" + ((Decimal)(100 * prgbar.Value) / prgbar.Maximum).ToString() + " tamamlandı!";
                        
                    }));
                }, i);
                


            }
            timer1.Stop(); }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private delegate void EventHandle();
        public EventLevel ReadEvent(int i)
        {
            
            EventLevel evnt = stack.Pop();

            Thread.Sleep(5000);

                
                
            
           
            return evnt;
        }

       

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ticks++;
                Invoke((Action)(() =>
                {
                    lbl_timer.Text = ticks.ToString();
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
