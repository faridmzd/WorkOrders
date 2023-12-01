using Microsoft.AspNetCore.Components;
using W.O.Web.Models;

namespace W.O.Web.Pages.Parts
{
    public class PartsListBase:ComponentBase
    {
        [Parameter]
        public IEnumerable<PartDTO> Parts { get; set; } = new List<PartDTO>();
    }

}
