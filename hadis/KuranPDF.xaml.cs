using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hadis
{
    public partial class KuranPDF : ContentPage
    {
        private readonly string _localPdfPath;
        private FileStream _pdfStream;
        public KuranPDF()
        {
            InitializeComponent();
            _localPdfPath = Path.Combine(FileSystem.AppDataDirectory, "kuran.pdf");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Yield(); // UI önce çizilsin
            await KuranPDFYukleAsync();
        }

        private async Task KuranPDFYukleAsync()
        {
            SetLoadingState(true);

            try
            {
                await İlkAcılıstaKopyala();

                // PDF'i sınıf seviyesinde aç
                _pdfStream = new FileStream(_localPdfPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                // Stream ile PDF'i yükle
                pdfViewer.LoadDocument(_pdfStream); // async yerine sync kullanıyoruz, lazy ile performans kaybolmaz
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"PDF yüklenirken bir sorun oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        // Sayfa kapatıldığında Stream'i kapat
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Stream'i kapat
            _pdfStream?.Dispose();
            _pdfStream = null;
        }


        private void SetLoadingState(bool isLoading)
        {
            pdfViewer.IsVisible = !isLoading;
        }

        private async Task İlkAcılıstaKopyala()
        {
            if (File.Exists(_localPdfPath))
                return;

            using (Stream assetStream = await FileSystem.OpenAppPackageFileAsync("kuran.pdf"))
            {
                using (FileStream fileStream = File.Create(_localPdfPath))
                {
                    await assetStream.CopyToAsync(fileStream);
                }
            }
        }
    }
}

