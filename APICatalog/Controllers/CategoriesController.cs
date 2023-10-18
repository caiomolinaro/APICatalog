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
            return _dbContext.Categories.AsNoTracking().Include(p => p.Products).ToList(); //o include inclui os produtos

        }

        //get que retorna todas as categorias
        [HttpGet]
        public ActionResult<IEnumerable<CategoryModel>> Get()
        {
            return _dbContext.Categories.AsNoTracking().ToList(); //so retorna as categorias, o as no tracking retorna uma nova consulta e não é
                                                                  //amarzenada no cache pelo context, otimiza a consulta e
                                                                  //só deve ser usado em consultas somente leitura (get)
        }

        //get que retorna a categoria pelo id
        [HttpGet("{id:int}", Name = "ObtainCategory")]
        public ActionResult<CategoryModel> Get(int id)
        {
            var category = _dbContext.Categories.AsNoTracking().FirstOrDefault(p => p.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        //post que cria uma categoria
        [HttpPost]
        public ActionResult Post(CategoryModel category)
        {
            if (category is null)
            {
                return BadRequest();
            }

            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return new CreatedAtRouteResult("ObtainCategory", new { id = category.CategoryId }, category);
        }

        //put que atualiza uma categoria
        [HttpPut("{id:int}")]
        public ActionResult put(int id, CategoryModel category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _dbContext.Entry(category).State = EntityState.Modified; //altera o estado pra modified/modificado
            _dbContext.SaveChanges();
            return Ok(category);
        }

        //metodo delete que deleta uma categoria
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(p => p.CategoryId == id);
            
            if (category == null)
            {
                return NotFound();
            }

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
