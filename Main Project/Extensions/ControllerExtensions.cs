using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Main_Project.Extensions
{
    // Extension class to provide additional functionalities to controllers.
    public static class ControllerExtensions
    {
        // Renders a view as a string. Can render both partial and full views.
        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            // Use the current action name if the viewName is not specified.
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            // Set the model for the view data.
            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                // Retrieve the view engine from the services.
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                // Find the view using the view engine.
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                // Throw an exception if the view cannot be found.
                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                // Create a view context for rendering.
                ViewContext viewContext = new(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                // Render the view to the string writer.
                await viewResult.View.RenderAsync(viewContext);

                // Return the rendered view as a string.
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}