using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Play.Catalog.Service.Dtos;
using System;
using System.Linq;
using Play.Catalog.Service.Repositories;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Controllers
{   
    [ApiController]
    [Route("items")]//controla las rutas que empiezan con "/items"
    public class ItemsController : ControllerBase
    {

        private readonly IItemsRepository itemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]   //funcion metodo get
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());//convierto la entidad a un DTO

            return items;
        }

        //si hace un GET /items/un_id
        [HttpGet("{id}")]   
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);
            
            if(item == null)
            {
                return NotFound();
            }
            
            return item.AsDto();
        }

        [HttpPost]
        //ActionResult, devuelve estados HTTP, codigo 200, 404, 500, etc o si no uno de los Dto
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            
            var item = new Item{
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            //Los primeros 2 parametros son unos "headers" de la response, mientras que el ultimo es la respuesta en si (el elemento creado)
            return CreatedAtAction(nameof(GetByIdAsync), new {id = item.Id}, item);
        }

        [HttpPut("{id}")]   //IActionResult, no devolvemos nada en especifico, simplemente un estado http, en este caso 204 no content
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            
            var existingItem = await itemsRepository.GetAsync(id);

            if(existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            
            var item = await itemsRepository.GetAsync(id);

            if(item == null)
            {
                return NotFound();
            }

            await itemsRepository.RemoveAsync(item.Id);

            return NoContent();

        }

    }
}

