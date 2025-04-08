using System.Collections.Generic;
using fareShare.Models;

namespace fareShare.Repositories;

public interface IBillRepositories
{
    Bill CreateBill(Bill bill);
    Bill GetBill(int id);
    Bill UpdateBill(Bill bill);
    void DeleteBill(int id);
    List<Bill> GetBillsByUserId(int userId);
    List<BillLink> GetBillLinksByUserId(int userId);
    BillLink CreateBillLink(BillLink billLink);
    void DeleteBillLink(int billLinkId);
}