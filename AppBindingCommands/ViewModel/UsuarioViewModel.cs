using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppBindingCommands.ViewModel
{
    internal class UsuarioViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

       

        private string name = string.Empty;
        public string DisplayName => $"Nome digitado: {name}";

        private string DisplayMessage = string.Empty;



        public string Name
        {
            get => name;


            set
            {

                if (name == null)
                    return;

                name = value;
                OnPropertyChanged(nameof(name));
                OnPropertyChanged(DisplayName);

            }

       
    }

        public string DisplayMessage1 { get => DisplayMessage;
            set 
            {
                if(DisplayMessage == null)
                    return;

                DisplayMessage = value;
                OnPropertyChanged(nameof(DisplayName));
            
                
            }
        }

        public ICommand ShowMessageCommand { get; set; }

        public void ShowMessage()
        {
            DateTime data = Preferences.Get("dtAtual", DateTime.MinValue);
            DisplayMessage = $"Boa noite {Name}, hoje é {data}";
        }

        public UsuarioViewModel()
        {
            ShowMessageCommand = new Command(ShowMessage);
            CountCommand = new Command(async () => await CountCharacteres());
            CleanCommand = new Command(async() => await CleanConfirmation());
            OptionCommand = new Command((async () => await ShowOptions());
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public async Task CountCharacteres()
        {
            string nameLenght =
                string.Format("Seu nome tem {0} letras", name.Length);

            await Application.Current.
                MainPage.DisplayAlert("Informação ", nameLenght, "Ok");

        }

        public ICommand CountCommand { get; }
        public ICommand CleanCommand { get; }
        public ICommand OptionCommand { get; }

        public async Task CleanConfirmation()
        {
            if (await Application.Current.MainPage.
                DisplayAlert("COnfirmação", "Confirma Limpeza de Dados", "Yes", "No"));
            {
                name = string.Empty;
                DisplayMessage = string.Empty;
                OnPropertyChanged(name);
                OnPropertyChanged(nameof(Name));

                await Application.Current.MainPage.
                    DisplayAlert("Informação", "Limpeza realizada com sucesso!", "Ok");
            }

        }

        public async Task ShowOptions()
        {
            string result = await Application.Current.MainPage.
                DisplayActionSheet("Selecione uma opção: ", "", "Cancelar", "Limpar",
                "Contar Caracteres", "Exibir Saudação");

            if (result != null)
            {
                if (result.Equals("Limpar")) 
                    await CleanConfirmation();
                if (result.Equals("Contar Caracteres")) ;
                    await CleanConfirmation();
                if (result.Equals("Exibir Saudação")) ;
                    ShowMessage();
            }
        }


    }
}
