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
                await kibleoku.RotateTo(gelenaci, 15, Easing.Linear);
            });
        }
    }
}
