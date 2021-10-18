using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SKPLager.API.Configurations;
using SKPLager.API.Helpers;
using SKPLager.API.Hubs;
using SKPLager.API.Services;
using SKPLager.API.Services.Repos;
using SKPLager.API.Services.Storage;
using SKPLager.Shared.Configurations;
using SKPLager.Shared.IHubs;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Controllers
{

    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepo imageRepo;
        private readonly ICurrentUserDepartmentService currentUserDepartment;
        private readonly IHubContext<InventoryHub, IInventoryPushHub> _hubContext;
        private readonly IStorageService storage;

        public ImageController(ICurrentUserDepartmentService currentUserDepartment, IHubContext<InventoryHub, IInventoryPushHub> hubContext, IImageRepo imageRepo, IStorageService storage)
        {
            this.currentUserDepartment = currentUserDepartment;
            _hubContext = hubContext;
            this.imageRepo = imageRepo;
            this.storage = storage;
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpPost(ApiRoutes.Image.Create)]
        public async Task<IActionResult> CreateImage([FromForm] IFormFile file , [FromForm] string imageName, [FromForm] int departmentId)
        {
            if (!currentUserDepartment.IsInDepartment(departmentId))
            {
                return BadRequest("Not in department");
            }
            Image image = new Image();
            image.Name = imageName;
            image.DepartmentId = departmentId;
            image.Uri = await storage.UploadImageToStorage(file);
            await imageRepo.AddAsync(image);
            await imageRepo.SaveAsync();
            await _hubContext.Clients.Group(Group.Images(departmentId)).CreatedImage(image);
            return Ok(image);
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpDelete(ApiRoutes.Image.Delete)]
        public async Task<IActionResult> DeleteImage(int imageId)
        {

            if (!imageRepo.Any(x => x.Id == imageId && currentUserDepartment.Ids.Contains(x.DepartmentId)))
            {
                return BadRequest("Not in department");
            }
            var image = await imageRepo.GetAsync(imageId);
            var departmentId = image.DepartmentId;
            imageRepo.Remove(image);
            await imageRepo.SaveAsync();
            await _hubContext.Clients.Groups(Group.Images(departmentId)).DeletedImage(imageId);
            return Ok();
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Image.GetAll)]
        public async Task<IActionResult> GetImages([FromQuery] Pagination pagination)
        {            
            return Ok(await imageRepo.GetAsPagedList(pagination));
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Image.GetOne)]
        public async Task<IActionResult> GetImage(int imageId)
        {
            if (!imageRepo.Any(x => x.Id == imageId && currentUserDepartment.Ids.Contains(x.DepartmentId)))
            {
                return BadRequest("Not in department");
            }
            var inventory = await imageRepo.GetAsync(imageId);
            return Ok(inventory);
        }

    }
}
