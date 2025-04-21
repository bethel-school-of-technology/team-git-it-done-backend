using fareShare.Models;
using fareShare.Repositories;
using fareShare.Repository;
using Microsoft.AspNetCore.Mvc;

namespace fareShare.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillController : ControllerBase
{
    private readonly ILogger<BillController> _logger;
    private readonly IBillRepository _billRepository;

    public BillController(ILogger<BillController> logger, IBillRepository repository)
    {
        _logger = logger;
        _billRepository = repository;
    }

    //Use this to get all bills filtered to a user.
    // GET: api/Bill/user/3
    [HttpGet("user/{userId}")]
    public IActionResult GetBillsByUser(int userId)
    {
        try
        {
            var bills = _billRepository.GetBillsByUserId(userId);
            return Ok(bills);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get bills for user {userId}");
            return NotFound(ex.Message);
        }
    }

    //Use this to get the full detail of a single bill
    // GET: api/Bill/5
    [HttpGet("{id}")]
    public IActionResult GetBill(int id)
    {
        try
        {
            var bill = _billRepository.GetBill(id);
            return Ok(bill);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get bill with id {id}");
            return NotFound(ex.Message);
        }
    }

    //Use this to create a bill, will automatically link the creator to the bill
    // POST: api/Bill
    [HttpPost]
    public IActionResult CreateBill([FromBody] CreateBillDto dto)
    {
        try
        {
            var bill = new Bill
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CreatorId = dto.CreatorId
            };

            var created = _billRepository.CreateBill(bill);

            return CreatedAtAction(nameof(GetBill), new { id = created.BillId }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create bill");
            return BadRequest(ex.Message);
        }
    }

    // Use this to link a user to a bill
    // POST: api/Bill/link
    [HttpPost("link")]
    public IActionResult CreateBillLink([FromBody] BillLink billLink)
    {
        try
        {
            var created = _billRepository.CreateBillLink(billLink);
            return Ok(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create bill link");
            return BadRequest(ex.Message);
        }
    }

    //Use this to update a bill's details.
    // PUT: api/Bill/5
    [HttpPut("{id}")]
    public IActionResult UpdateBill(int id, [FromBody] UpdateBillDto updatedBillDto)
    {
        var existingBill = _billRepository.GetBill(id);
        if (existingBill == null)
        {
            return NotFound("Bill not found.");
        }

        existingBill.Name = updatedBillDto.Name;
        existingBill.Description = updatedBillDto.Description;
        existingBill.Price = updatedBillDto.Price;

        try
        {
            var updatedBill = _billRepository.UpdateBill(existingBill);
            return Ok(updatedBill);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to update bill with id {id}");
            return StatusCode(500, "Internal server error");
        }
    }

    //Use this to settle part or all of a bill for a user.
    // PUT: api/Bill/settle/7
    [HttpPut("settle/{billLinkId}")]
    public IActionResult SettleBill(int billLinkId, float amount)
    {
        try
        {
            _billRepository.SettleBill(billLinkId, amount);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to settle bill link with id {billLinkId}");
            return NotFound(ex.Message);
        }
    }

    //Use this to Delete a bill, not the link between a user and a bill.
    // DELETE: api/Bill/5
    [HttpDelete("{id}")]
    public IActionResult DeleteBill(int id)
    {
        try
        {
            _billRepository.DeleteBill(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete bill with id {id}");
            return NotFound(ex.Message);
        }
    }

    //Use this to delete the link between a user and a bill, not the bill itself.
    // DELETE: api/Bill/link/7
    [HttpDelete("link/{billLinkId}")]
    public IActionResult DeleteBillLink(int billLinkId)
    {
        try
        {
            _billRepository.DeleteBillLink(billLinkId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete bill link with id {billLinkId}");
            return NotFound(ex.Message);
        }
    }
}
