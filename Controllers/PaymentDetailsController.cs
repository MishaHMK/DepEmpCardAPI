using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DepEmpCardAPI.Models;

namespace DepEmpCardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PaymentDetailsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/PaymentDetails
        [HttpGet]
        public IQueryable<PaymentDetailDTO> GetPaymentDetails()
        {

            var employees = from e in _context.PaymentDetails
                            select new PaymentDetailDTO()
                            {
                                PaymentDetailId = e.PaymentDetailId,
                                CardOwnerId = e.CardOwnerId,
                                CardNumber = e.CardNumber,
                                ExpirationDate = e.ExpirationDate,
                                SecurityCode = e.SecurityCode
                            };

            return employees;
        }

        // GET: api/PaymentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetailDTO>> GetPaymentDetail(int id)
        {
            var paymentDetail = await _context.PaymentDetails.Select(e =>
              new PaymentDetailDTO()
              {
                  PaymentDetailId = e.PaymentDetailId,
                  CardOwnerId = e.CardOwnerId,
                  CardNumber = e.CardNumber,
                  ExpirationDate = e.ExpirationDate,
                  SecurityCode = e.SecurityCode
              }
            ).SingleOrDefaultAsync(b => b.PaymentDetailId == id);
            if (paymentDetail == null)
            {
                return NotFound();
            }

            return Ok(paymentDetail);
        }

        // PUT: api/PaymentDetails
        [HttpPut]
        public async Task<IActionResult> PutPaymentDetail(PaymentDetail paymentDetail)
        {
            _context.Entry(paymentDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentDetailExists(paymentDetail.PaymentDetailId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult("Updated Successfully");
        }

        // POST: api/PaymentDetails
        [HttpPost]
        public async Task<ActionResult<PaymentDetail>> PostPaymentDetail(PaymentDetail paymentDetail)
        {
    
                _context.PaymentDetails.Add(paymentDetail);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPaymentDetail",
                    new { id = paymentDetail.PaymentDetailId }, paymentDetail);
            
    
        }

        // DELETE: api/PaymentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentDetail(int id)
        {
            var paymentDetail = await _context.PaymentDetails.FindAsync(id);
            if (paymentDetail == null)
            {
                return NotFound();
            }

            _context.PaymentDetails.Remove(paymentDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: api/PaymentDetails/emp/1
        [HttpGet("emp/{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetPDEmployee(int id)
        {
            var paymentDetail = _context.PaymentDetails.SingleOrDefault(p => p.PaymentDetailId== id);

            var employee = await _context.Employees.Select(e =>
               new EmployeeDTO()
               {
                   EmployeeId = e.EmployeeId,
                   EmployeeName = e.EmployeeName,
                   EmployeeSurname = e.EmployeeSurname,
                   DepartmentName = e.DepartmentName,
                   DateOfJoining = e.DateOfJoining,
                   PhotoFileName = e.PhotoFileName
               }
             ).SingleOrDefaultAsync(b => b.EmployeeId == paymentDetail.CardOwnerId);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);


        }

        private bool PaymentDetailExists(int id)
        {
            return _context.PaymentDetails.Any(e => e.PaymentDetailId == id);
        }
    }
}
