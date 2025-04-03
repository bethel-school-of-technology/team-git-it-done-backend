using fareShare.Models;
using fareShare.Migrations;

namespace fareShare.Repositories;

public class BillRepository : IBillRepositories
{
	private readonly BillDbContext _context;

    public BillRepository(BillDbContext context)
    {
         _context = context;
    }

    public Bill CreateBill(Bill bill)
    {
        throw new NotImplementedException();
    }

    public BillLink CreateBillLink(BillLink billLink)
    {
        throw new NotImplementedException();
    }

    public void DeleteBill(int id)
    {
        throw new NotImplementedException();
    }

    public void DeleteBillLink(int billLinkId)
    {
        throw new NotImplementedException();
    }
    public Bill GetBill(int id)
    {
        throw new NotImplementedException();
    }

    public List<BillLink> GetBillLinksByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public List<Bill> GetBillsByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public Bill UpdateBill(Bill bill)
    {
        throw new NotImplementedException();
    }
}

