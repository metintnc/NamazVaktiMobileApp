using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Syncfusion.Maui.PdfViewer;

namespace hadis
{
    public partial class Kuran : ContentPage
    {
        private readonly string _localPdfPath;

        public Kuran()
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
                // PDF dosyasını kopyala (ilk açılış için)
                await İlkAcılıstaKopyala();

                // PDF dosyasını arka planda aç
                FileStream pdfStream = await Task.Run(() =>
                    new FileStream(_localPdfPath, FileMode.Open, FileAccess.Read, FileShare.Read));

                // UI thread'inde PDF'i yükle
                pdfViewer.LoadDocument(pdfStream);
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
