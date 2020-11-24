using Bakery.Core.DTOs;
using Bakery.Wpf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bakery.Wpf.ViewModels
{
    public class EditCreateProductViewModel : BaseViewModel
    {
        private ProductDto _product;
        private bool createMode = false;
        
        public EditCreateProductViewModel(IWindowController controller, ProductDto product) : base(controller)
        {
            _product = product;
            if (_product == null)
            {
                createMode = true;
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }
    }
}
