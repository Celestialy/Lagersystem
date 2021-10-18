using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SKPLager.API.Configurations;
using SKPLager.API.Hubs;
using SKPLager.API.Services;
using SKPLager.Shared.Helpers;
using SKPLager.Shared.IHubs;
using SKPLager.Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Controllers
{
    [ApiController]
    public class BarcodePrinterController : ControllerBase
    {
        private readonly IHubContext<PrintHub, IPrintHub> _hubContext;
        private readonly ICurrentUserDepartmentService currentUserDepartment;
        public BarcodePrinterController(IHubContext<PrintHub, IPrintHub> hubContext, ICurrentUserDepartmentService currentUserDepartment)
        {
            _hubContext = hubContext;
            this.currentUserDepartment = currentUserDepartment;
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpPost(ApiRoutes.Department.Printer.Print)]
        public async Task<IActionResult> PrintBarcode(int departmentId, [FromBody] PrintDTO print)
        {
            if (!currentUserDepartment.IsInDepartment(departmentId))
            {
                return BadRequest("User not in the same department as printer");
            }
            if (string.IsNullOrWhiteSpace(print.Barcode))
            {
                return BadRequest("Barcode is empty or null");
            }
            await _hubContext.Clients.Group(departmentId.ToString()).PrintRequest(print.Barcode);
            return NoContent();
        }
    }
}
