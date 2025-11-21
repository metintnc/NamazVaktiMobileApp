using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hadis
{
    public partial class kible : ContentPage
    {
        private Pusula compass;
        public kible()
        {
            InitializeComponent();
            compass = new Pusula();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await compass.KontrolEt();
            compass.AciDegisti += KıbleOkunuDondur;
        }
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            compass.PusulaDurdur();
            compass.AciDegisti -= KıbleOkunuDondur;
        }

        public void KıbleOkunuDondur(double gelenaci)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if(gelenaci < 180)
                {
                    await kibleoku.RotateTo(gelenaci, 150, Easing.Linear);
                }
                else
                {
                    await kibleoku.RotateTo(gelenaci -360, 150, Easing.Linear);
                }
                    AciDegeri.Text = $"{360 - gelenaci:F0}°";
                int a = Convert.ToInt32(gelenaci);
                if(360 - a == 0)
                {
                    AciDegeri.TextColor = Colors.Gold;
                }
                else
                {
                    AciDegeri.ClearValue(Label.TextColorProperty);
                }
            });
        }
    }
}
