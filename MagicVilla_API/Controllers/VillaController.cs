using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dtos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected ApiResponse<VillaDto> _response;

        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepositorio, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepositorio;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las villas");
                var lista = await _villaRepo.ObtenerTodo();

                _response.ListaDatos = _mapper.Map<List<VillaDto>>(lista);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }

        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Id=0 al intentar buscar una villa");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "El idientificador de la entidad es nula." };
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                //var villa = VillaStore.VillaList.Find(f => f.Id == id);
                var villa = await _villaRepo.Obtener(f => f.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "El idientificador de la entidad es nula." };
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Dato = _mapper.Map<VillaDto>(villa);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> CrearVilla([FromBody] VillaCrearDto villa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { string.Join(", ", ModelState.Select(x => x.Value.Errors).Where(y => y.Count() > 0).SelectMany(z => z.ToArray())) };
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "No se recepcionó los datos para cargar." };
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                if (await _villaRepo.Obtener(v => v.Nombre.ToLower().Equals(villa.Nombre.ToLower())) != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "El nombre ya existe entre los datos cargados" };
                    _response.IsExitoso = false; 
                    return BadRequest(_response);
                }

                Villa modelo = _mapper.Map<Villa>(villa);

                await _villaRepo.Crear(modelo);

                _response.Dato = _mapper.Map<VillaDto>(modelo);
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetVilla", _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_response);
            }


        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    _response.ErrorMessages = new List<string>() { "No se recepcionó el identificador del dato a ser eliminado." };
                    return BadRequest(_response);
                }
                var villa = await _villaRepo.Obtener(f => f.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    _response.ErrorMessages = new List<string>() { "No se encontró el dato a ser eliminado." };
                    return NotFound(_response);
                }
                await _villaRepo.Remover(villa); //no existe asincrono

                _response.StatusCode = HttpStatusCode.NoContent;


                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_response);
            }


        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> UpdateVilla(int id, [FromBody] VillaDto villa)
        {
            try
            {
                if (villa == null || id != villa.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    _response.ErrorMessages = new List<string>() { "Los datos enviados para actualizar son erroneos. Verifique." };
                    return BadRequest(_response);
                }

                Villa modelo = _mapper.Map<Villa>(villa);

                await _villaRepo.Actualizar(modelo); //no es asincrono

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_response);
            }

        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0) { return BadRequest(); }
                var villa = await _villaRepo.Obtener(f => f.Id == id, false);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    _response.ErrorMessages = new List<string>() { "No se encontro la Villa a Actualizar." };
                    return NotFound(_response);

                }
                VillaDto villaDto = _mapper.Map<VillaDto>(villa);

                patchDto.ApplyTo(villaDto, ModelState);

                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { string.Join(", ", ModelState.Select(x => x.Value.Errors).Where(y => y.Count() > 0).SelectMany(z => z.ToArray())) };
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                villa = _mapper.Map<Villa>(villaDto);

                await _villaRepo.Actualizar(villa);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_response);
            }


        }
    }
}
