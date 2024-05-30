using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;

namespace InternalPortal.Web.Services
{
    public static class ModelStateExtensions
    {
        public static IDictionary ToSerializedDictionary(this ModelStateDictionary modelState)
        {
            return modelState.ToDictionary(
                k => k.Key,
                v => v.Value.Errors.Select(x => x.ErrorMessage).ToArray()
    );
        }
    }
}
