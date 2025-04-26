using System.Collections.Generic;
using fareShare.Models;

namespace fareShare.Repository;

public interface IBillRepository
{
    Bill CreateBill(Bill bill);
    Bill GetBill(int id);
    Bill UpdateBill(Bill bill);
    void DeleteBill(int id);
    List<Bill> GetBillsByUserId(int userId);
    BillLink CreateBillLink(BillLink billLink);
    void DeleteBillLink(int billLinkId);
    void SetBillShare (int billId);
    void SettleBill (int billId, float amount);
    float GetBillShare (int billId, int userId);
}
