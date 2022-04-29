namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Ingredients.Features;
using SharedKernel.Dtos.RecipeManagement.Ingredient;
using RecipeManagement.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

[ApiController]
[Route("api/ingredients")]
[ApiVersion("1.0")]
public class IngredientsController: ControllerBase
{
    private readonly IMediator _mediator;

    public IngredientsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all Ingredients.
    /// </summary>
    /// <response code="200">Ingredient list returned successfully.</response>
    /// <response code="400">Ingredient has missing/invalid values.</response>
    /// <response code="500">There was an error on the server while creating the Ingredient.</response>
    /// <remarks>
    /// Requests can be narrowed down with a variety of query string values:
    /// ## Query String Parameters
    /// - **PageNumber**: An integer value that designates the page of records that should be returned.
    /// - **PageSize**: An integer value that designates the number of records returned on the given page that you would like to return. This value is capped by the internal MaxPageSize.
    /// - **SortOrder**: A comma delimited ordered list of property names to sort by. Adding a `-` before the name switches to sorting descendingly.
    /// - **Filters**: A comma delimited list of fields to filter by formatted as `{Name}{Operator}{Value}` where
    ///     - {Name} is the name of a filterable property. You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if LikeCount or CommentCount is >10
    ///     - {Operator} is one of the Operators below
    ///     - {Value} is the value to use for filtering. You can also have multiple values (for OR logic) by using a pipe delimiter, eg.`Title@= new|hot` will return posts with titles that contain the text "new" or "hot"
    ///
    ///    | Operator | Meaning                       | Operator  | Meaning                                      |
    ///    | -------- | ----------------------------- | --------- | -------------------------------------------- |
    ///    | `==`     | Equals                        |  `!@=`    | Does not Contains                            |
    ///    | `!=`     | Not equals                    |  `!_=`    | Does not Starts with                         |
    ///    | `>`      | Greater than                  |  `@=*`    | Case-insensitive string Contains             |
    ///    | `&lt;`   | Less than                     |  `_=*`    | Case-insensitive string Starts with          |
    ///    | `>=`     | Greater than or equal to      |  `==*`    | Case-insensitive string Equals               |
    ///    | `&lt;=`  | Less than or equal to         |  `!=*`    | Case-insensitive string Not equals           |
    ///    | `@=`     | Contains                      |  `!@=*`   | Case-insensitive string does not Contains    |
    ///    | `_=`     | Starts with                   |  `!_=*`   | Case-insensitive string does not Starts with |
    /// </remarks>
    [ProducesResponseType(typeof(IEnumerable<IngredientDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [HttpGet(Name = "GetIngredients")]
    public async Task<IActionResult> GetIngredients([FromQuery] IngredientParametersDto ingredientParametersDto)
    {
        var query = new GetIngredientList.IngredientListQuery(ingredientParametersDto);
        var queryResponse = await _mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a single Ingredient by ID.
    /// </summary>
    /// <response code="200">Ingredient record returned successfully.</response>
    /// <response code="400">Ingredient has missing/invalid values.</response>
    /// <response code="500">There was an error on the server while creating the Ingredient.</response>
    [ProducesResponseType(typeof(IngredientDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [HttpGet("{id:guid}", Name = "GetIngredient")]
    public async Task<ActionResult<IngredientDto>> GetIngredient(Guid id)
    {
        var query = new GetIngredient.IngredientQuery(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Ingredient record.
    /// </summary>
    /// <response code="201">Ingredient created.</response>
    /// <response code="400">Ingredient has missing/invalid values.</response>
    /// <response code="500">There was an error on the server while creating the Ingredient.</response>
    [ProducesResponseType(typeof(IngredientDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPost(Name = "AddIngredient")]
    public async Task<ActionResult<IngredientDto>> AddIngredient([FromBody]IngredientForCreationDto ingredientForCreation)
    {
        var command = new AddIngredient.AddIngredientCommand(ingredientForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetIngredient",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Ingredient.
    /// </summary>
    /// <response code="204">Ingredient updated.</response>
    /// <response code="400">Ingredient has missing/invalid values.</response>
    /// <response code="500">There was an error on the server while creating the Ingredient.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [HttpPut("{id:guid}", Name = "UpdateIngredient")]
    public async Task<IActionResult> UpdateIngredient(Guid id, IngredientForUpdateDto ingredient)
    {
        var command = new UpdateIngredient.UpdateIngredientCommand(id, ingredient);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Ingredient record.
    /// </summary>
    /// <response code="204">Ingredient deleted.</response>
    /// <response code="400">Ingredient has missing/invalid values.</response>
    /// <response code="500">There was an error on the server while creating the Ingredient.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [HttpDelete("{id:guid}", Name = "DeleteIngredient")]
    public async Task<ActionResult> DeleteIngredient(Guid id)
    {
        var command = new DeleteIngredient.DeleteIngredientCommand(id);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Creates one or more Ingredient records.
    /// </summary>
    /// <response code="201">Ingredient List created.</response>
    /// <response code="400">Ingredient List has missing/invalid values.</response>
    /// <response code="500">There was an error on the server while creating the list of Ingredient.</response>
    [ProducesResponseType(typeof(IEnumerable<IngredientDto>), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPost("batch", Name = "AddIngredientList")]
    public async Task<ActionResult<IngredientDto>> AddIngredient([FromBody]IEnumerable<IngredientForCreationDto> ingredientForCreation,
        [FromQuery(Name = "recipeId"), BindRequired] Guid recipeId)
    {
        var command = new AddIngredientList.AddIngredientListCommand(ingredientForCreation, recipeId);
        var commandResponse = await _mediator.Send(command);

        return Created("GetIngredient", commandResponse);
    }

    // endpoint marker - do not delete this comment
}
