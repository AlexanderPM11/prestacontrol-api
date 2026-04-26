using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prestacontrol.Application.DTOs;
using Prestacontrol.Domain.Entities;
using Prestacontrol.Domain.Interfaces;
using AutoMapper;

namespace Prestacontrol.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _unitOfWork.Clients.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ClientDto>>(clients));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(_mapper.Map<ClientDto>(client));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, _mapper.Map<ClientDto>(client));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientDto clientDto)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null) return NotFound();

            _mapper.Map(clientDto, client);
            _unitOfWork.Clients.Update(client);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
