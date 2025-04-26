using System;
using System.Collections.Generic;
using System.Linq;
using fareShare.Migrations;
using fareShare.Models;
using fareShare.Repository;
using Microsoft.EntityFrameworkCore;

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
        if (bill == null) throw new ArgumentNullException(nameof(bill));

        bill.SharedPrice = bill.Price;
        _context.Bill.Add(bill);
        _context.SaveChanges();

        if (bill.CreatorId == null)
            throw new InvalidOperationException("CreatorId must be set when creating a bill.");

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

        this.SetBillShare(billLink.BillId);

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
        var bill = _context.Bill.Include(b => b.BillLinks).FirstOrDefault(b => b.BillId == id);

        if (bill == null)
        {
            throw new ArgumentNullException(nameof(bill));
        }

        return bill;
    }

    public List<Bill> GetBillsByUserId(int userId)
    {
        var bills = _context
            .BillLink.Where(bl => bl.UserId == userId)
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

    public void SetBillShare(int billId)
    {
        var bill = _context.Bill.Include(b => b.BillLinks).FirstOrDefault(b => b.BillId == billId);

        if (bill == null)
            throw new ArgumentNullException(nameof(bill));

        var totalShare = bill.BillLinks.Count;

        var sharePerUser = bill.Price / totalShare;

        bill.SharedPrice = sharePerUser;

        _context.Bill.Update(bill);
        _context.SaveChanges();
    }

    public void SettleBill(int billLinkId, float amount)
    {
        var billLink = _context.BillLink.Find(billLinkId);
        if (billLink == null)
        {
            throw new ArgumentNullException(nameof(billLink));
        }

        billLink.Settled += amount;
        _context.SaveChanges();
    }

    public float GetBillShare(int billId, int userId)
    {
        var billLink = _context.BillLink.FirstOrDefault(bl => bl.BillId == billId && bl.UserId == userId);
        if (billLink == null)
        {
            throw new ArgumentNullException(nameof(billLink));
        }

        return billLink.Settled;
    }
}
