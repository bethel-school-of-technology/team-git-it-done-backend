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

    // POST: api/Bill
    [HttpPost]
    public IActionResult CreateBill([FromBody] Bill bill)
    {
        try
        {
            var created = _billRepository.CreateBill(bill);
            return CreatedAtAction(nameof(GetBill), new { id = created.BillId }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create bill");
            return BadRequest(ex.Message);
        }
    }

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

    // PUT: api/Bill/5
    [HttpPut("{id}")]
    public IActionResult UpdateBill(int id, [FromBody] Bill updatedBill)
    {
        if (id != updatedBill.BillId)
        {
            return BadRequest("ID mismatch.");
        }

        try
        {
            var bill = _billRepository.UpdateBill(updatedBill);
            return Ok(bill);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to update bill with id {id}");
            return NotFound(ex.Message);
        }
    }

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
