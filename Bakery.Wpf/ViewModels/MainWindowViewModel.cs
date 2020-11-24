using Bakery.Core.DTOs;
using Bakery.Persistence;
using Bakery.Wpf.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Bakery.Wpf.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        private ObservableCollection<ProductDto> _products;

        public ObservableCollection<ProductDto> Products
        {
            get => _products;
            set 
            {
                _products = value;
                OnPropertyChanged();
            }
        }



        public MainWindowViewModel(IWindowController controller) : base(controller)
        {

            LoadCommands();
        }

        private void LoadCommands()
        {

        }


        public static async Task<MainWindowViewModel> Create(IWindowController controller)
        {
            var model = new MainWindowViewModel(controller);
            await model.LoadProducts();
            return model;
        }

        /// <summary>
        /// Produkte laden. Produkte können nach Preis gefiltert werden. 
        /// Preis liegt zwischen from und to
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private async Task LoadProducts()
        {
            await using UnitOfWork uow = new UnitOfWork();

            //var products = uow.Products.Get
            Products = new ObservableCollection<ProductDto>();

        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }
    }
}
