using Solar.Models;
using Solar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Solar.Views;
using System.IO;
using Plugin.Media.Abstractions;
using Plugin.Media;

namespace Solar.ViewModels
{
    public class LogInViewModel:BaseViewModel
    {

        public ICommand SignUpCommand { get; set; }
        public ICommand LogInCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public LogInViewModel(IAuthenticationService authentication)
        {
            User = new User();
            SignUpCommand = new Command(async () => await SignUp());
            RegisterCommand = new Command(async () => await Register());
            LogInCommand = new Command(async () => await LogIn());
            TakePhotoCommand = new Command(async () => await TakePhotoMethod());
            this.authentication = authentication;
        }

        private async Task Register()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SignUp());
        }

        private async Task TakePhotoMethod()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                //Directory = Path.Combine(FileSystem.AppDataDirectory, "Photos"),
                SaveToAlbum = false,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 92,
                DefaultCamera = CameraDevice.Front
            });
            if (file == null)
                return;
            ImageSrc = ImageSource.FromStream(() =>
            {
                User.PhotoPath = file.Path;
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            /*var file = await TakePhoto.TakeAsync();
            User.PhotoPath = file.Path;
            ImageSrc = ImageSource.FromStream(() => {
                var stream =file.GetStream();
                file.Dispose();
                return stream;
            }) ;
            await Application.Current.MainPage.DisplayAlert("File Location", file.AlbumPath, "OK");*/
        }

        private async Task LogIn()
        {
            IsBusy = true;
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {
                if(await authentication.UserExist(Email))
                {
                    IsMsgUserVisible = false;
                    if (await authentication.LogIn(Email, Password))
                    {
                        IsMsgPassVisible = false;
                        IsBusy = false;
                        await Application.Current.MainPage.DisplayAlert("Inicio de sesión exitoso", $"Bienvenido {await authentication.GetNameByEmail(Email)}", "Ok");
                        Application.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        IsMsgPassVisible = true;
                        IsBusy = false;
                        await Application.Current.MainPage.DisplayAlert("Error", "Contraseña Incorrecta", "Volver a intentar");
                        MsgPass = "Contraseña Incorrecta";
                    }
                }
                else
                {
                    IsBusy = false;
                    IsMsgUserVisible = true;
                    MsgUser = "El usuario no está registrado, por favor registrese";
                }
            }
        }

        private async Task SignUp()
        {
            if (string.IsNullOrEmpty(User.LastName) ||
                 string.IsNullOrEmpty(User.Name) ||
                 string.IsNullOrEmpty(User.Password) ||
                 string.IsNullOrEmpty(User.Password) || !User.Password.Equals(RptPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Verifique que los campos sean correctos y que las contraseñas coincidand", "Ok");
            }
            else
            {
                if (await authentication.SignUp(User))
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Usuario registrado correctamente", "Ok");
                return;
            }
        }

        private ImageSource imageSrc = Application.Current.Resources["UserWhite"] as FontImageSource;

        public ImageSource ImageSrc
        {
            get { return imageSrc; }
            set { SetProperty(ref (imageSrc), value); }
        }


        public string RptPassword { get; set; }

        private bool isMsgUserVisible;

        public bool IsMsgUserVisible
        {
            get { return isMsgUserVisible; }
            set { SetProperty(ref (isMsgUserVisible), value); }
        }
        private bool isMsgPassVisible;

        public bool IsMsgPassVisible
        {
            get { return isMsgPassVisible; }
            set { SetProperty(ref (isMsgPassVisible), value); }
        }

        private string msgUser;

        public string MsgUser
        {
            get => msgUser;
            set => SetProperty(ref (msgUser), value);
        }

        private string msgPass;

        public string MsgPass
        {
            get => msgPass;
            set => SetProperty(ref (msgPass), value);
        }


        public User User { get; set; }

        private string password;
        public string Password 
        {
            get => password;
            set => SetProperty(ref (password), value);
        }

        private string email;
        private readonly IAuthenticationService authentication;

        public string Email 
        {
            get => email;
            set => SetProperty(ref (email), value);
        }
    }
}
