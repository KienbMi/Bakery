using Bakery.Core.DTOs;
using Bakery.Persistence;
using Bakery.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bakery.Wpf.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        private ObservableCollection<ProductDto> _products;
        private string _priceFrom;
        private string _priceTo;

        public ObservableCollection<ProductDto> Products
        {
            get => _products;
            set 
            {
                _products = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AvgPriceOfProducts));
            }
        }

        public string PriceFrom
        {
            get => _priceFrom;
            set 
            { 
                _priceFrom = value;
                ValidateViewModelProperties();
            }
        }

        public string PriceTo
        {
            get => _priceTo;
            set 
            { 
                _priceTo = value;
                ValidateViewModelProperties();
            }
        }

        public double AvgPriceOfProducts
        {
            get
            {
                if (Products == null)
                    return 0;
                if (Products.Count == 0)
                    return 0;

                return Math.Round((Products.Sum(p => p.Price) / Products.Count),2);
            }
        }

        public ProductDto SelectedProduct { get; set; }

        public ICommand CmdFilter { get; set; }
        public ICommand CmdNewProduct { get; set; }
        public ICommand CmdEditProduct { get; set; }

        public MainWindowViewModel(IWindowController controller) : base(controller)
        {

            LoadCommands();
        }

        private void LoadCommands()
        {
            CmdFilter = new RelayCommand(async _ => await LoadProducts(), _ => IsValid);
            CmdNewProduct = new RelayCommand(async _ => await ÊditCreateProduct(), _ => true);
            CmdEditProduct = new RelayCommand(async _ => await ÊditCreateProduct(SelectedProduct), _ => SelectedProduct != null);
        }

        private async Task ÊditCreateProduct(ProductDto product = null)
        {
            var window = new EditCreateProductViewModel(Controller, product);
            Controller.ShowWindow(window, true);

            var selectedProduct = SelectedProduct;
            await LoadProducts();

            if (selectedProduct != null)
            {
                SelectedProduct = Products.FirstOrDefault(p => p.Id == selectedProduct.Id);
                OnPropertyChanged(nameof(SelectedProduct));
            }

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

            double priceFrom = 0;
            double priceTo = 0;

            double.TryParse(PriceFrom, out priceFrom);
            double.TryParse(PriceTo, out priceTo);

            var products = await uow.Products.GetWithFilterAsync(priceFrom, priceTo);
            Products = new ObservableCollection<ProductDto>(products);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(PriceFrom) && !double.TryParse(PriceFrom, out var dummy))
            {
                yield return new ValidationResult("Input is no valid price", new string[] { nameof(PriceFrom) });
            }
            if (!string.IsNullOrWhiteSpace(PriceTo) && !double.TryParse(PriceTo, out dummy))
            {
                yield return new ValidationResult("Input is no valid price", new string[] { nameof(PriceTo) });
            }
        }
    }
}
