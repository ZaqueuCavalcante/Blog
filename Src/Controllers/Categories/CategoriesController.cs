using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Blog.Configurations.ControllersConfigurations;
using static Blog.Configurations.AuthorizationConfigurations;
using Blog.Services.Categories;

namespace Blog.Controllers.Categories;

[ApiController, Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(
        ICategoriesService categoriesService
    ) {
        _categoriesService = categoriesService;
    }

    /// <summary>
    /// Register a new category.
    /// </summary>
    [HttpPost, Authorize(Roles = AdminRole)]
    [ProducesResponseType(typeof(CategoryOut), 201)]
    public async Task<IActionResult> PostCategory(CategoryIn dto)
    {
        var category = await _categoriesService.CreateCategory(dto.Name, dto.Description);

        return Created($"/categories/{category.Id}", new CategoryOut(category));
    }

    /// <summary>
    /// Returns a category.
    /// </summary>
    [HttpGet("{id}"), AllowAnonymous]
    [ProducesResponseType(typeof(CategoryOut), 200)]
    public async Task<ActionResult> GetCategory(int id)
    {
        var category = await _categoriesService.GetCategory(id);

        return Ok(new CategoryOut(category));
    }

    /// <summary>
    /// Returns all the categories.
    /// </summary>
    [HttpGet, AllowAnonymous]
    [ProducesResponseType(typeof(List<CategoryOut>), 200)]
    [ResponseCache(CacheProfileName = TwoMinutesCacheProfile)]
    public async Task<ActionResult> GetCategories()
    {
        var categories = await _categoriesService.GetCategories();

        return Ok(categories.Select(c => new CategoryOut(c)).ToList());
    }
}
