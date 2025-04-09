using fareShare.Models;
using fareShare.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using fareShare.Repository;

namespace fareShare.Repository;

public class BillRepository : IBillRepository
{
        private readonly BillDbContext _context;

        public BillRepository(BillDbContext context)
        {
            _context = context;
        }

        public Bill CreateBill(Bill bill)
        {
            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }

            // Save the Bill
            _context.Bill.Add(bill);
            _context.SaveChanges();

            // Automatically link the creator to the Bill using BillLink
            if (bill.CreatorId == null)
            {
                throw new InvalidOperationException("CreatorId must be set when creating a bill.");
            }

            var billLink = new BillLink
            {
                BillId = bill.BillId,
                UserId = bill.CreatorId.Value
            };

            _context.BillLink.Add(billLink);
            _context.SaveChanges();

            return bill;
        }

        public BillLink CreateBillLink(BillLink billLink)
        {
            if (billLink == null)
            {
                throw new ArgumentNullException(nameof(billLink));
            }

            _context.BillLink.Add(billLink);
            _context.SaveChanges();

            return billLink;
        }

        public void DeleteBill(int id)
        {
            var bill = _context.Bill.Find(id);
            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }

            _context.Bill.Remove(bill); // Will also delete BillLinks due to cascade
            _context.SaveChanges();
        }

        public void DeleteBillLink(int billLinkId)
        {
            var link = _context.BillLink.Find(billLinkId);
            if (link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }

            _context.BillLink.Remove(link);
            _context.SaveChanges();
        }

        public Bill GetBill(int id)
        {
            var bill = _context.Bill
                .Include(b => b.BillLinks)
                .FirstOrDefault(b => b.BillId == id);

            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }

            return bill;
        }

        public List<Bill> GetBillsByUserId(int userId)
        {
            var bills = _context.BillLink
                .Where(bl => bl.UserId == userId)
                .Include(bl => bl.Bill)
                .Select(bl => bl.Bill)
                .ToList();

            return bills;
        }

        public Bill UpdateBill(Bill updatedBill)
        {
            if (updatedBill == null)
            {
                throw new ArgumentNullException(nameof(updatedBill));
            }

            var existingBill = _context.Bill.Find(updatedBill.BillId);

            if (existingBill == null)
            {
                throw new ArgumentNullException(nameof(existingBill));
            }

            existingBill.Name = updatedBill.Name;
            existingBill.Description = updatedBill.Description;
            existingBill.Price = updatedBill.Price;

            _context.SaveChanges();
            return existingBill;
    }
}
