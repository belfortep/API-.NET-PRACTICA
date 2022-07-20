using System;
using System.ComponentModel.DataAnnotations;
//Aca guardo los Dtos, teniendo uno para distintas necesidades
namespace Play.Catalog.Service.Dtos
{   
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
    //poniendo adelante como un decorador puedo decir si algo es requerido, un rango, y demas
    public record CreateItemDto([Required] string Name, string Description,[Range(1, 100000)] decimal Price);
    //estos decoradores ya envian el error 400 cuando no se cumplen y dan una pequeña descripcion
    public record UpdateItemDto([Required] string Name, string Description,[Range(1, 100000)] decimal Price);

}