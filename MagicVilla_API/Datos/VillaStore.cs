using MagicVilla_API.Modelos.Dtos;

namespace MagicVilla_API.Datos
{
    public static class VillaStore
    {
        public static List<VillaDto> VillaList = new() {
            new VillaDto { Id = 1,Nombre="Vista a la Piscina",Ocupantes=3,Metros2=50},
            new VillaDto { Id = 2,Nombre="Vista a la Playa",Ocupantes=4,Metros2=80}
        };
    }
}
