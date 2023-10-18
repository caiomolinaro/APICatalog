using APICatalog.Context;
using APICatalog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //dependencia do contexto do banco de dados
        private readonly AppDbContext _dbContext;

        //injetando a classe de contexto
        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //metodo get que retorna todos os produtos
        [HttpGet]
        public ActionResult <IEnumerable<ProductModel>> Get()
        {
            //acessando os produtos no banco de dados
            var products = _dbContext.Products.AsNoTracking().ToList();

            //verifica se os produtos não são nulls
            if(products is null)
            {
                return NotFound("Produtos não encontrados"); //equivalente ao codigo http 404 - não encontrado
            }

            return products;
        }

        //metodo get que retorna um produto pelo id
        [HttpGet("{id:int}", Name="ObtainProduct")]//aceita apenas int no endpoint
        public ActionResult<ProductModel> Get(int id)
        {
            //armazena o produto por id na var product
            var product = _dbContext.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);

            if (product is null)
            {
                return NotFound("Produto não encontrado");
            }
            return product;
        }

        //metodo post que cria um novo produto
        [HttpPost]
        public ActionResult Post(ProductModel product) 
        {
            if(product is null)
            {
                return BadRequest();
            }

            //cria e persiste o novo produto na tabela
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return new CreatedAtRouteResult("ObtainProduct", new { id = product.ProductId }, product); // retorna o http 202 created

        }

        //metodo put que atualiza um produto existente
        [HttpPut("{id:int}")] //valor mapeado para o parametro do metodo id int
        public ActionResult Put(int id, ProductModel product)
        {
            if(id != product.ProductId)
            {
                return BadRequest(); //se for diferente o id retorna o badrequest http 400
            }

            //identifica que vai ser modificado e salva
            _dbContext.Entry(product).State = EntityState.Modified; //identifica que vai ser modificado e salva
            _dbContext.SaveChanges();

            return Ok(product);
        }

        //metodo delete que deleta um produto existente
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            //localiza o produto pelo id
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == id);   

            //verifica se o produto é null
            if(product is null)
            {
                return NotFound("Produto não foi localizado"); //http 404
            }

            //se o produto for localizado então é removido

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return Ok(); //retorna o http 200
        }
    }
}
