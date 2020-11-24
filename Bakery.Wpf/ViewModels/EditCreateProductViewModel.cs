using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Bakery.Persistence;
using Bakery.Wpf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bakery.Wpf.ViewModels
{
    public class EditCreateProductViewModel : BaseViewModel
    {
        private ProductDto _product;
        private bool _createMode = false;

        public string Title => (_createMode) ? "Produkt anlegen" : "Produkt bearbeiten";

        private string _productNumber;
        private string _productName;
        private string _productPrice = "0";

        public string ProductNumber
        {
            get => _productNumber;
            set 
            { 
                _productNumber = value;
                OnPropertyChanged();
                ValidateViewModelProperties();          
            }
        }

        [MinLength(1, ErrorMessage = "Produktname muss mindestens 1 Zeichen lang sein")]
        [MaxLength(20, ErrorMessage = "Produktname darf maximal 20 Zeichen lang sein")]
        public string ProductName
        {
            get => _productName;
            set 
            { 
                _productName = value;
                OnPropertyChanged();
                ValidateViewModelProperties();
            }
        }

        public string ProductPrice
        {
            get => _productPrice;
            set 
            { 
                _productPrice = value;
                OnPropertyChanged();
                ValidateViewModelProperties();
            }
        }

        public ICommand CmdSave { get; set; }
        public ICommand CmdUndo { get; set; }


        public EditCreateProductViewModel(IWindowController controller, ProductDto product) : base(controller)
        {
            LoadCommands();
            _product = product;
            if (_product == null)
            {
                _createMode = true;
            }
            else
            {
                ProductNumber = _product.ProductNr;
                ProductName = _product.Name;
                ProductPrice = _product.Price.ToString();
            }
        }

        private void LoadCommands()
        {
            CmdSave = new RelayCommand(async _ => await SaveProduct(), _ => IsValid);
            CmdUndo = new RelayCommand(_ => Undo(), _ => true);
        }

        private void Undo()
        {
            if (_createMode)
            {
                ProductNumber = string.Empty;
                ProductName = string.Empty;
                ProductPrice = "0";
                return;
            }

            ProductNumber = _product.ProductNr;
            ProductName = _product.Name;
            ProductPrice = _product.Price.ToString();
        }

        private async Task SaveProduct()
        {

            await using UnitOfWork uow = new UnitOfWork();

            if (_createMode)
            {
                Product product = new Product
                {
                    ProductNr = ProductNumber,
                    Price = double.Parse(ProductPrice),
                    Name = ProductName
                };
                uow.Products.Add(product);
            }
            else
            {
                var productInDb = await uow.Products.GetByIdAsync(_product.Id);
                productInDb.ProductNr = ProductNumber;
                productInDb.Price = double.Parse(ProductPrice);
                productInDb.Name = ProductName;
            }

            try
            {
                await uow.SaveChangesAsync();
                Controller.CloseWindow(this);
            }
            catch (ValidationException validationException)
            {
                DbError = validationException.ValidationResult.ToString();
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!double.TryParse(ProductPrice, out var dummy))
            {
                yield return new ValidationResult("Input is no valid price", new string[] { nameof(ProductPrice) });
            }
        }
    }
}
