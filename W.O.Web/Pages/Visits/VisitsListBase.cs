using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using W.O.Web.Models;

namespace W.O.Web.Pages.Visits
{
    public class VisitsListBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<VisitDTO> Visits { get; set; }

        [Parameter]
        public Guid Id { get; set; }
    }
}
