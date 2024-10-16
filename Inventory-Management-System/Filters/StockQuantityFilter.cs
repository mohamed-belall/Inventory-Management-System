using Inventory_Management_System.Repository;
using Inventory_Management_System.Repository.repo;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventory_Management_System.Filters
{
    public class StockQuantityFilter : IAsyncActionFilter
    {
        private readonly IProductRepository productRepository;
        private readonly AlertController alertController;
        private readonly IAlertRepository alertRepository;

        public StockQuantityFilter(IProductRepository productRepository,AlertController alertController,IAlertRepository alertRepository)
        {
            this.productRepository = productRepository;
            this.alertController = alertController;
            this.alertRepository = alertRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            List<int>? LowProductsIds = productRepository.GetLowQuantitesProducts();//1 4 5 6 7

            List<int>? alertedProducts = alertRepository.GetAllAlertedProductsIds();//1 4 

            List<int>? notAlertedProducts = LowProductsIds.Except(alertedProducts).ToList();

            if (notAlertedProducts != null)
            {
                foreach (int id in notAlertedProducts)
                {
                    
                    AlertWithEmployeesProductViewModel alertWithEmployeesProductViewModel = new AlertWithEmployeesProductViewModel();
                    alertWithEmployeesProductViewModel.AlertDate = DateTime.Now;
                    alertWithEmployeesProductViewModel.AlertQuantityLevel = GlobalVariables.threshold * GlobalVariables.AlertFactor;
                    alertWithEmployeesProductViewModel.IsResolved = false;
                    alertWithEmployeesProductViewModel.EmployeeId = 4;
                    alertWithEmployeesProductViewModel.ProductId = id;

                    alertController.SaveAdd(alertWithEmployeesProductViewModel);

                }
            }

            await next();
        }
    }
}
