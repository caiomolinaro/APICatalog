using APICatalog.Context;
using APICatalog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CategoriesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //get que retorna as categorias com os produtos
        [HttpGet("products")] //define uma outra rota, tem que informar o nome do controlador/products para acessar
        public ActionResult<IEnumerable<CategoryModel>> GetCategoriesProducts()
        {
            try
            {
                return _dbContext.Categories.AsNoTracking().Include(p => p.Products).ToList(); //o include inclui os produtos
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema");
            }

        }

        //get que retorna todas as categorias
        [HttpGet]
        public ActionResult<IEnumerable<CategoryModel>> Get()
        {
            try
            { 
                return _dbContext.Categories.AsNoTracking().ToList(); //so retorna as categorias, o as no tracking retorna uma nova consulta e não é
                                                                      //amarzenada no cache pelo context, otimiza a consulta e
            }                                                         //só deve ser usado em consultas somente leitura (get)
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema");
            }
        }

        //get que retorna a categoria pelo id
        [HttpGet("{id:int}", Name = "ObtainCategory")]
        public ActionResult<CategoryModel> Get(int id)
        {
            try
            {
                var category = _dbContext.Categories.AsNoTracking().FirstOrDefault(p => p.CategoryId == id);

                if (category == null)
                {
                    return NotFound($"A categoria com o id: {id} não foi encontrada");
                }
                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema");
            }
        }

        //post que cria uma categoria
        [HttpPost]
        public ActionResult Post(CategoryModel category)
        {
            try
            {
                if (category is null)
                {
                    return BadRequest();
                }

                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();

                return new CreatedAtRouteResult("ObtainCategory", new { id = category.CategoryId }, category);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema");
            }
        }

        //put que atualiza uma categoria
        [HttpPut("{id:int}")]
        public ActionResult put(int id, CategoryModel category)
        {
            try
            {
                if (id != category.CategoryId)
                {
                    return BadRequest("Os dados informados, são inválidos");
                }

                _dbContext.Entry(category).State = EntityState.Modified; //altera o estado pra modified/modificado
                _dbContext.SaveChanges();
                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema");
            }
        }

        //metodo delete que deleta uma categoria
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var category = _dbContext.Categories.FirstOrDefault(p => p.CategoryId == id);

                if (category == null)
                {
                    return NotFound($"A categoria com o id: {id} não foi encontrada");
                }

                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema");
            }
        }
    }
}
