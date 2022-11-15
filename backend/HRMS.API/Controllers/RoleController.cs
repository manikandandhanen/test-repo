using AutoMapper;
using HRMS.API.Models;
using HRMS.Entities;
using HRMS.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class RoleController : ApiController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(
          RoleManager<ApplicationRole> roleManager,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        //GET: api/role
        [HttpGet]
        public ActionResult GetAll()
        {
            var roles = _roleManager.Roles;
            if(roles == null)
            {
                return NotFound();
            }

            return Ok( _mapper.Map<IEnumerable<RoleDto>>(roles));
        }

        //GET: api/role/id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            if (id == null)
            {
               return ModelStateErrorResponseError(nameof(id), $"{nameof(id)} cannot be null or empty");
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RoleDto>(role));
        }

        // POST: api/role
        [HttpPost]
        public async Task<ActionResult> Add(/*to protect overpost enabled property binding*/
            [Bind("RoleName")][FromBody] RoleDto model)
        {
            var applicationRole = new ApplicationRole { Name = model.RoleName };

            IdentityResult createResult = await _roleManager.CreateAsync(applicationRole);

            if(!createResult.Succeeded)
            {
                return IdentityErrorResponseError(createResult.Errors);
            }

            return  Ok(_mapper.Map<RoleDto>(applicationRole));
        }

        // PUT: api/role
        [HttpPut]
        public async Task<ActionResult> Update([Bind("RoleName", "RoleId")][FromBody] RoleDto model)
        {
            if (string.IsNullOrWhiteSpace(model.RoleId))
            {
                return ModelStateErrorResponseError(nameof(model.RoleId), $"{nameof(model.RoleId)} cannot be null or empty");
            }         

            ApplicationRole roleToUpdate = await _roleManager.FindByIdAsync(model.RoleId);

            if (roleToUpdate == null)
            {
                return NotFound();
            }

            roleToUpdate.Name = model.RoleName;

            IdentityResult updateResult = await _roleManager.UpdateAsync(roleToUpdate);

            if (!updateResult.Succeeded)
            {
                return IdentityErrorResponseError(updateResult.Errors);
            }

            return Ok(updateResult.Succeeded);
        }        

        // DELETE: api/role/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            if (id == null)
            {
                return ModelStateErrorResponseError(nameof(id), $"{nameof(id)} cannot be null or empty");
            }

            ApplicationRole role = await _roleManager.FindByIdAsync(id);

            if(role == null)
            {
                return NotFound();
            }

            IdentityResult deleteResult = await _roleManager.DeleteAsync(role);

            if(!deleteResult.Succeeded)
            {
                return IdentityErrorResponseError(deleteResult.Errors);
            }

            return Ok(deleteResult.Succeeded);
        }
    }
}
