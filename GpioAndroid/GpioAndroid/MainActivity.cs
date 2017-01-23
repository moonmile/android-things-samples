using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Com.Google.Android.Things.Pio;
using System.Threading.Tasks;

namespace GpioAndroid
{
    [Activity(Label = "GpioAndroid", MainLauncher = true, Icon = "@drawable/icon")]
public class MainActivity : Activity
{
    TextView text1;
    Gpio mLedGpio;


    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.Main);

        // Get our button from the layout resource,
        // and attach an event to it
        Button button = FindViewById<Button>(Resource.Id.MyButton);
        button.Click += Button_Click;
        text1 = FindViewById<TextView>(Resource.Id.textView1);

        PeripheralManagerService service = new PeripheralManagerService();
        try
        {
            var pinName = "BCM4";   // RPi3 
            // String pinName = BoardDefaults.getGPIOForLED();
            mLedGpio = service.OpenGpio(pinName);
            mLedGpio.SetDirection(Gpio.DirectionOutInitiallyLow);
        }
        catch 
        {     
        }

        var task = new Task(async() => {
            while (true)
            {
                await Task.Delay(1000);
                mLedGpio.Value = !mLedGpio.Value;
                System.Diagnostics.Debug.WriteLine("led: {0}", mLedGpio.Value);

                RunOnUiThread(() => {
                    if (mLedGpio.Value)
                    {
                        text1.SetBackgroundColor(Android.Graphics.Color.LightPink);
                    }
                    else
                    {
                        text1.SetBackgroundColor(Android.Graphics.Color.White);
                    }
                });
            }
        });
        task.Start();
    }

    private void Button_Click(object sender, EventArgs e)
    {
        mLedGpio.Value = !mLedGpio.Value;
    }
}
}

