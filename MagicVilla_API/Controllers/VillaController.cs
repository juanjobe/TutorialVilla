using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dtos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepositorio, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepositorio;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obteniendo todas las villas");
            var lista = await _villaRepo.ObtenerTodo();
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(lista));
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Id=0 al intentar buscar una villa");
                return BadRequest();
            }
            //var villa = VillaStore.VillaList.Find(f => f.Id == id);
            var villa = await _villaRepo.Obtener(f => f.Id == id);
            if (villa == null)
            {
                return NotFound();
            }


            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCrearDto villa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest(villa);
            }

            if (await _villaRepo.Obtener(v => v.Nombre.ToLower().Equals(villa.Nombre.ToLower())) != null)
            {
                ModelState.AddModelError("Nombre", "El nombre ya existe entre los datos cargados");
                return BadRequest(ModelState);
            }


            //if (villa.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}


            Villa modelo = _mapper.Map<Villa>(villa);
            //esto se reemplaza con el automapping
            //Villa modelo = new()
            //{
            //    Nombre = villa.Nombre,
            //    Detalle = villa.Detalle,
            //    ImagenUrl = villa.ImagenUrl,
            //    Ocupantes = villa.Ocupantes,
            //    Tarifa = villa.Tarifa,
            //    Metros2 = villa.Metros2,
            //    Amenidad = villa.Amenidad,
            //};

            await _villaRepo.Crear(modelo);

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, villa);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _villaRepo.Obtener(f => f.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            await _villaRepo.Remover(villa); //no existe asincrono

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaDto villa)
        {
            if (villa == null || id != villa.Id) { return BadRequest(villa); }

            Villa modelo = _mapper.Map<Villa>(villa);

            await _villaRepo.Actualizar(modelo); //no es asincrono

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if (patchDto == null || id == 0) { return BadRequest(); }
            var villa = await _villaRepo.Obtener(f => f.Id == id, false);
            if (villa == null)
            {
                return NotFound("No se encontro la Villa a Actualizar.");
            }
            VillaDto villaDto = _mapper.Map<VillaDto>(villa);

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }


            villa = _mapper.Map<Villa>(villaDto);


            await _villaRepo.Actualizar(villa);

            return NoContent();
        }
    }
}
